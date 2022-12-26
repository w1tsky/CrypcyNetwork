using Crypcy.ApplicationCore.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crypcy.NodeUI.NodeLogic
{
    public class UiImpl : IUserInterface
    {
        protected static int _indexCounter { get; set; } = 0;
        static ConcurrentDictionary<string, string> _nodes = new ConcurrentDictionary<string, string>();

        public event Action<string, string>? OnSendMessageRequest;
        public event Action<int> OnStartNode;

        public event Action<string> OnNodeConnected;
        public event Action<string> OnNodeDiconnected;
        public event Action<string, string> OnMessageReceived;

        public void NodeConnectedNotification(string node)
        {
            var index = (++_indexCounter).ToString();
            _nodes[index] = node;
            OnNodeConnected?.Invoke(node);
        }

        public void NodeDiconnectedNotification(string node)
        {
            var n = GetNodePairIndexByNode(node);
            _nodes.Remove(n.Key, out var _);
            OnNodeDiconnected?.Invoke(n.Key);
        }

        public void ShowMessage(string node, string message)
        {
            var n = GetNodePairIndexByNode(node);
            OnMessageReceived.Invoke(n.Value, message);
        }

        protected KeyValuePair<string, string> GetNodePairIndexByNode(string node)
        {
            return _nodes.Single(n => n.Value == node);
        }

    }
}
