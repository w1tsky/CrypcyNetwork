using Crypcy.ApplicationCore.Contracts;
using Crypcy.ApplicationCore.TempMessageSolution.MessagesTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore.TempMessageSolution.MessageHandles
{
    internal class TextMessageHandler : MessageHandler<TextMessage>
	{
		private readonly IUserInterface _userInterface;

		public TextMessageHandler(IUserInterface userInterface)
		{
			_userInterface = userInterface;
		}

		public override void Handle(TextMessage message, string node)
		{
			_userInterface.ShowMessage(node, message.Text);
		}
	}
}
