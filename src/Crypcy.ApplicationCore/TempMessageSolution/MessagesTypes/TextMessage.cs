using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore.TempMessageSolution.MessagesTypes
{
    internal class TextMessage : BaseMessage
	{
		public TextMessage() : base(MessageKind.Text)
		{
		}

		public string Text { get; init; }
	}
}
