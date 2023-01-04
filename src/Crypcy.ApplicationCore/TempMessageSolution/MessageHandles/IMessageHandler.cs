using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypcy.ApplicationCore.TempMessageSolution.MessagesTypes;

namespace Crypcy.ApplicationCore.TempMessageSolution.MessageHandles
{
    public interface IMessageHandler
    {
        public void Handle(object message, string node);

        public Type GetProcessedMessageType();
    }
}
