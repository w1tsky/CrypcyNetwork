using Crypcy.ApplicationCore;
using Crypcy.Communication.Network;
using Crypcy.NodeUI.Models;
using Crypcy.NodeUI.NodeLogic;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.NodeUI.Components
{
    public class NodeUiComponent : ComponentBase
    {
        [Inject]
        public UiImpl _ui { get; set; }

        public List<string> _nodes = new List<string>();
        public List<NodeMessage> _messages = new List<NodeMessage>();

        protected override void OnInitialized()
        {
            var _ui = new UiImpl();
            var network = new TcpNetwork();
            var nodes = new Nodes(network, _ui);

            _ui.OnNodeConnected += NodeConnected;
            _ui.OnNodeDiconnected += NodeDiconnected;
            _ui.OnMessageReceived += ReceiveMessage;
        }


        private void NodeConnected(string node)
        {
            _nodes.Add(node);
        }

        private void NodeDiconnected(string node)
        {
            _nodes.Remove(node);
        }

        private void ReceiveMessage(string node, string message)
        {
            _messages.Add(new NodeMessage
            {
                NodeSender = node,
                NodeMessageText = message,
            });
        }





    }
}
