using Crypcy.ApplicationCore.Contracts;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;


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
        public event Action<string, HashSet<string>>? OnCreateGroupRequest;
        public event Action<string, string>? OnSendGroupMessageRequest;

        public void NodeConnectedNotification(string node)
		{
			var index = (++_indexCounter).ToString();
			_nodes[index] = node;

			Console.WriteLine($"Node connected: NodeIndex:{index}; Node:{node};");
		}

		public void NodeDiconnectedNotification(string node)
		{
			var n = GetNodePairIndexByNode(node);
			Console.WriteLine($"Node disconected: NodeIndex:{n.Key}; Node:{n.Value};");
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
            foreach (var n in _nodeGroups[group])
                Console.WriteLine($"Node:{n};");
            return ;
        }

        protected KeyValuePair<string, string> GetNodePairIndexByNode(string node)
		{
			return _nodes.Single(n => n.Value == node);
		}

		internal void NewInput(string input, CancellationTokenSource cts)
		{
            // help
            var helpReg = new Regex(@"^help$", RegexOptions.IgnoreCase);
            if (helpReg.IsMatch(input))
            {
                Console.WriteLine("Type \"start:port\" to start node with port.");
                Console.WriteLine("Type \"list\" to show list of nodes.");
                Console.WriteLine("Type \"connect HOSTNAME_OR_IP:PORT\" to connect to node.");
                Console.WriteLine("Type \"exit\" to exit app.");
                Console.WriteLine("For sending message you must write message in specific format:");
                Console.WriteLine("Template: Nmsg:your message");
                Console.WriteLine("N is number of target node");
                return;
            }

            // exit app
            var regExit = new Regex(@"^exit$", RegexOptions.IgnoreCase);
            if (regExit.IsMatch(input))
            {
                Console.WriteLine("exiting...");
                cts.Cancel();
                return;
            }

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
                int port = int.Parse(strtValues[1].Value);
                if (port < 0 || port > 65535)
                {
                    Console.WriteLine("Invalid port number, valid range is 0 to 65535");
                    return;
                }
                OnStartNode?.Invoke(port);
                Console.WriteLine($"Node started on port {port} ...");
                return;
            }

            // stop node 
            var regStop = new Regex(@"^stop$", RegexOptions.IgnoreCase);
            if (regStop.IsMatch(input))
            {
                OnStopNode?.Invoke();
                Console.WriteLine("Node stopping...");
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
            var connectToNode = new Regex(@"^connect (?'hostnameOrIp'([a-zA-Z0-9-\.]+)|(\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b)):(?'port'\d+)$", RegexOptions.IgnoreCase);
            if (connectToNode.IsMatch(input))
            {
                var values = connectToNode.Matches(input)[0].Groups;
                var hostnameOrIp = values["hostnameOrIp"].Value;
                if (int.TryParse(values["port"].Value, out var port))
                {
                    OnConnectToNodeRequest?.Invoke(hostnameOrIp, port);
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid port. Please specify a valid port number.");
                }
                OnConnectToNodeRequest?.Invoke(hostnameOrIp, port);
            }
            else
            {
                Console.WriteLine("Invalid input. Please use the format 'connect HOSTNAME_OR_IP:PORT' to specify the hostname or IP and port of the node to connect to.");
            }

        }


    }
}
