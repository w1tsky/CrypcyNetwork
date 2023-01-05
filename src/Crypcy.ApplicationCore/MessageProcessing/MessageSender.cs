using Crypcy.ApplicationCore.Contracts.Network;
using Crypcy.ApplicationCore.MessagesTypes;
using System.Text.Json;

namespace Crypcy.ApplicationCore.MessageProcessing
{
	public class MessageSender : IMessageSender
	{
		private readonly ISendData _sendMessage;

		public MessageSender(ISendData sendMessage)
		{
			_sendMessage = sendMessage;
		}

		public ValueTask SendMessageAsync(string node, BaseMessage message)
		{
			return _sendMessage.SendMessageAsync(node, JsonSerializer.SerializeToUtf8Bytes((object)message));
		}
	}
}
