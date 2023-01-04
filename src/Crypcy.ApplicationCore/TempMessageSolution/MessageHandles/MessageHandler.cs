using Crypcy.ApplicationCore.TempMessageSolution.MessagesTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore.TempMessageSolution.MessageHandles
{
	public abstract class MessageHandler<T> : IMessageHandler where T : BaseMessage
	{
		public abstract void Handle(T message, string node);

		void IMessageHandler.Handle(object message, string node) => Handle((T)message, node);

		Type IMessageHandler.GetProcessedMessageType() => typeof(T);
	}
}
