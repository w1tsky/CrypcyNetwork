using Crypcy.ApplicationCore.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.NodeUI.NodeLogic
{
    internal class NodeInit : IUserInterface
    {
        public event Action<string, string> OnSendMessageRequest;

        public void NodeConnectedNotification(string node)
        {
            throw new NotImplementedException();
        }

        public void NodeDiconnectedNotification(string node)
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(string node, string message)
        {
            throw new NotImplementedException();
        }
    }
}
