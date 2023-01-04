using Crypcy.ApplicationCore.Contracts.Network;
using Crypcy.ApplicationCore.TempMessageSolution.MessagesTypes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore.TempMessageSolution.MessageHandles
{
	public class FrontDataHandler
	{
		private readonly IReciveData _reciveData;
		private readonly ImmutableDictionary<Type, IMessageHandler> _massagesHandlers;

		public FrontDataHandler(IEnumerable<IMessageHandler> messagesHandlers, IReciveData reciveData)
		{
			_reciveData = reciveData;
			_massagesHandlers = messagesHandlers.ToImmutableDictionary(
				keySelector: messagesHandler => messagesHandler.GetProcessedMessageType());
			_reciveData.OnNewMessageRecived += ReciveData;
		}

		public void ReciveData(string node, byte[] data)
		{
			#region sub functions
			Type GetMessageType(MessageKind messageKind)
			{
				switch (messageKind)
				{
					case MessageKind.Text: return typeof(TextMessage);
					default: throw new Exception($"Not found MessageType for {messageKind}");
				}
			}
			#endregion

			var messageKind = JsonSerializer.Deserialize<BaseMessage>(data).MessageType;
			var messageType = GetMessageType(messageKind);
			var message = JsonSerializer.Deserialize(data, messageType)!;
			var messageHandler = _massagesHandlers[messageType];

			messageHandler.Handle(message, node);
		}
	}
}
