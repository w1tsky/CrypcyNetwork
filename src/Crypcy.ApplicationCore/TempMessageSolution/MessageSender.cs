using Crypcy.ApplicationCore.Contracts.Network;
using Crypcy.ApplicationCore.TempMessageSolution.MessagesTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore.TempMessageSolution
{
    public class MessageSender
	{
		private readonly ISendData _sendMessage;

		public MessageSender(ISendData sendMessage)
		{
			_sendMessage = sendMessage;
		}

		public void SendMessage(string node, BaseMessage message) 
		{
			_sendMessage.SendMessage(node, JsonSerializer.SerializeToUtf8Bytes((object)message));
		}
	}
}
