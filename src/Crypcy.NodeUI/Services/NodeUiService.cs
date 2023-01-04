using Crypcy.ApplicationCore.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crypcy.NodeUI.Services
{
    public class NodeUiService : IUserInterface
    {
        private List<string> _nodes = new List<string>();
        private Dictionary<string, ICollection<string>> _nodeGroups = new Dictionary<string, ICollection<string>>();

        public event Action<int> OnStartNode;
        public event Action OnStopNode;


        public event Action<string> OnNodeConnected;
        public event Action<string> OnNodeDisconnected;

        public event Action<string, int> OnConnectToNodeRequest;
        public event Action<string, string> OnSendMessageRequest;
        public event Action<string, HashSet<string>> OnCreateGroupRequest;
        public event Action<string, string> OnSendGroupMessageRequest;

        public event Action<string, string> OnMessageReceived;

        
        public void NodeStart(int port)
        {
            OnStartNode?.Invoke(port);
        }

        public void NodeStop()
        {
            OnStopNode?.Invoke();
        }

        public void NodeConnect(string ip, int port)
        {
            OnConnectToNodeRequest?.Invoke(ip, port);
        }

        public void NodeConnectedNotification(string node)
        {
            _nodes.Add(node);
            OnNodeConnected?.Invoke(node);
        }

        public void NodeDiconnectedNotification(string node)
        {
            _nodes.Remove(node);
            OnNodeDisconnected?.Invoke(node);
        }

        public void NodeGroupAdd(string nodeGroup, ICollection<string> nodes)
        {
            OnCreateGroupRequest?.Invoke(nodeGroup, nodes.ToHashSet());
            _nodeGroups.Add(nodeGroup, nodes);
        }

        public void NodeSendMessage(string node, string message)
        {
            OnSendMessageRequest?.Invoke(node, message);
        }
        public void NodeSendGroupMessage(string nodeGroup, string message)
        {
            OnSendGroupMessageRequest?.Invoke(nodeGroup, message);
        }


        public void ShowMessage(string node, string message)
        {
            OnMessageReceived?.Invoke(node, message);
        }



        public List<string> GetConnectedNodes()
        {
            return _nodes;
        }


    }
}
