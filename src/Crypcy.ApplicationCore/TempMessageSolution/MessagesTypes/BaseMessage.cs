using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore.TempMessageSolution.MessagesTypes
{
    public class BaseMessage
    {
        public MessageKind MessageType { get; private set; }

        public BaseMessage(MessageKind messageType) => MessageType = messageType;
    }
}
