using Crypcy.ApplicationCore.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crypcy.NodeConsole
{
    public class ConsoleImp : IUserInterface
    {
        protected static int _indexCounter { get; set; } = 0;
        static ConcurrentDictionary<string, string> _nodes = new ConcurrentDictionary<string, string>();
        
        public event Action<string, string>? OnSendMessageRequest;

        public void NewNodeConnectedNotification(string node)
        {
            var index = (++_indexCounter).ToString();
            _nodes[index] = node;

            Console.WriteLine($"New client connected: NodeIndex:{index}; Node:{node};");
        }

        public void NodeDiconnectedNotification(string node)
        {
            var n = GetNodePairIndexByNode(node);
            Console.WriteLine($"Client disconected: NodeIndex:{n.Key}; Node:{n.Value};");
            _nodes.Remove(n.Key, out var _);
        }

        public void ShowMessage(string node, string message)
        {
            var n = GetNodePairIndexByNode(node);
            Console.WriteLine($"MSG: {message} (NodeIndex:{n.Key}; Node:{n.Value})");
        }

        public void ShowConnectedNodes()
        {
            foreach(var n in _nodes)
                Console.WriteLine($"NodeIndex:{n.Key}; Node:{n.Value};");
        }
        protected KeyValuePair<string, string> GetNodePairIndexByNode(string node) 
        { 
            return _nodes.Single(n => n.Value == node);
        }

        internal void NewInput(string input) 
        {
            var r = new Regex(@"(\d+)msg:(.+)$");
            switch (input)
            {
                case "l":
                case "L":
                    ShowConnectedNodes();
                    break;
                case var value when r.IsMatch(value):             
                    var values = r.Matches(input)[0].Groups;
                    OnSendMessageRequest?.Invoke(_nodes[values[1].Value], values[2].Value);
                    break;
            }
        }
    }
}
