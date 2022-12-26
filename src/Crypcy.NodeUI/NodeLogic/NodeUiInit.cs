using Crypcy.ApplicationCore;
using Crypcy.ApplicationCore.Contracts;
using Crypcy.Communication.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.NodeUI.NodeLogic
{
    public class NodeUiInit : IUserInterface
    {

        public TcpNetwork _network;

        public UiImpl _ui;    

        public Nodes _nodes;

        public NodeUiInit()
        {
            _network = new TcpNetwork();
            _ui = new UiImpl();
            _nodes = new Nodes(_network, _ui);
        }


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

        public void StartNode(int port)
        {
            throw new NotImplementedException();
        }
    }
}
