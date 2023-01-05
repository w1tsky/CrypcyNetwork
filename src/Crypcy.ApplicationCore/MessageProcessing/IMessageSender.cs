using Crypcy.ApplicationCore.MessagesTypes;

namespace Crypcy.ApplicationCore.MessageProcessing
{
	public interface IMessageSender
	{
		ValueTask SendMessageAsync(string node, BaseMessage message);
	}
}