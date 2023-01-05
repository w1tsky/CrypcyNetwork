using Crypcy.ApplicationCore.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crypcy.NodeConsole
{
	public class ConsoleImp : IUserInterface
	{
		protected static int _indexCounter { get; set; } = 0;
		static ConcurrentDictionary<string, string> _nodes = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, List<string>> _nodeGroups = new ConcurrentDictionary<string, List<string>>();

		public event Action<int>? OnStartNode;
        public event Action? OnStopNode;
        public event Action<string, int>? OnConnectToNodeRequest;
        public event Action<string, string>? OnSendMessageRequest;
        public event Action<string, HashSet<string>> OnCreateGroupRequest;
        public event Action<string, string> OnSendGroupMessageRequest;


        public void NodeConnectedNotification(string node)
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
			foreach (var n in _nodes)
				Console.WriteLine($"NodeIndex:{n.Key}; Node:{n.Value};");
		}

        public void ShowGroup(string group)
        {
            throw new NotImplementedException();
        }

        protected KeyValuePair<string, string> GetNodePairIndexByNode(string node)
		{
			return _nodes.Single(n => n.Value == node);
		}

		internal void NewInput(string input)
		{
			// send message:
			var sendMessage = new Regex(@"^(\d+)msg:(.+)$", RegexOptions.IgnoreCase);
			if (sendMessage.IsMatch(input))
			{
				var values = sendMessage.Matches(input)[0].Groups;
				OnSendMessageRequest?.Invoke(_nodes[values[1].Value], values[2].Value);
				return;
			}

			// start node
			var regStart = new Regex(@"start:(\d+)", RegexOptions.IgnoreCase);
			if (regStart.IsMatch(input))
			{
				var strtValues = regStart.Matches(input)[0].Groups;
				OnStartNode?.Invoke(Int32.Parse(strtValues[1].Value));
				Console.WriteLine("Node working...");
				return;
			}

			// show connected list
			var showConnectedNodes = new Regex(@"^list$", RegexOptions.IgnoreCase);
			if (showConnectedNodes.IsMatch(input))
			{
				ShowConnectedNodes();
				return;
			}

            // connect to node
            var connectToNode = new Regex(@"^connect (?'ip'\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b):(?'port'\d+)$", RegexOptions.IgnoreCase);
            if (connectToNode.IsMatch(input))
            {
                var values = connectToNode.Matches(input)[0].Groups;
                var ip = values["ip"].Value;
                var port = int.Parse(values["port"].Value);
                OnConnectToNodeRequest?.Invoke(ip, port);
            }
            else
            {
                Console.WriteLine("Invalid input. Please use the format 'connect IP:PORT' to specify the IP and port of the node to connect to.");
            }
        }


    }
}
