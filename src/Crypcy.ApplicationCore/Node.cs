using Crypcy.ApplicationCore.Contracts;
using System.Net;

namespace Crypcy.ApplicationCore
{
	public class Node
	{
		protected HashSet<string> _nodes;
		protected NodeGroup _nodeGroups;


        private CancellationToken _token;
        private CancellationTokenSource _cts;

        private readonly ICommunication _communication;
		private readonly IUserInterface _userInterface;

		public Node(ICommunication communication, IUserInterface userInterface)
		{
			_nodes = new HashSet<string>();
			_nodeGroups = new NodeGroup(_nodes);

            _cts = new ();
            _token  = _cts.Token;

            _communication = communication;
			_userInterface = userInterface;

			_communication.OnNodeConnected += NodeConnected;
			_communication.OnNodeDisconnected += NodeDisconected;
			_communication.OnNewMessageRecived += MessegeRecived;
			_userInterface.OnSendMessageRequest += SendMessage;
			_userInterface.OnStartNode += NodeStart;
            _userInterface.OnConnectToNodeRequest += ConnectToNode;
			_userInterface.OnCreateGroupRequest += CreateNodeGroup;
			_userInterface.OnSendGroupMessageRequest += SendMessageToGroup;

		}

		protected void ConnectToNode(string ip, int port)
		{
			_communication.ConnectToNode(ip, port).Wait();
		}

		protected void NodeStart(int port)
		{
			_communication.StartAsync(port, _token);
		}

        protected void NodeStop()
        {
			_cts.Cancel();
        }

        protected void NodeConnected(string node)
		{
			lock (_nodes) _nodes.Add(node);
			_userInterface.NodeConnectedNotification(node);
		}
		protected void NodeDisconected(string node)
		{
			lock (_nodes) _nodes.Remove(node);
			_userInterface.NodeDiconnectedNotification(node);
		}

		protected void SendMessage(string node, string message)
		{
			_communication.SendMessageAsync(node, message);
		}

		protected void MessegeRecived(string node, string message)
		{
			_userInterface.ShowMessage(node, message);
		}

        protected void CreateNodeGroup(string groupName, HashSet<string> nodes)
        {
			_nodeGroups.AddGroup(groupName, nodes);
        }


        protected void AddNodeToGroup(string groupName, string node)
        {
            _nodeGroups.AddNodeToGroup(groupName, node);
        }

        protected void SendMessageToGroup(string groupName, string message)
        {
			Task.WaitAll(_nodeGroups.GetGroupNodes(groupName)
				.Select(n => _communication.SendMessageAsync(n, message))
				.ToArray());
        }

    }
}
