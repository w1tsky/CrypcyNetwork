using Crypcy.ApplicationCore.Contracts;
using System.Net;

namespace Crypcy.ApplicationCore
{
	public class Node
	{
		protected HashSet<string> _nodes;
		protected NodeGroup _nodeGroups;

		private readonly ICommunication _communication;
		private readonly IUserInterface _userInterface;

		public Node(ICommunication communication, IUserInterface userInterface)
		{
			_nodes = new HashSet<string>();
			_nodeGroups = new NodeGroup(_nodes);

            _communication = communication;
			_userInterface = userInterface;

			_communication.OnNodeConnected += NodeConnected;
			_communication.OnNodeDisconnected += NodeDisconected;
			_communication.OnNewMessageRecived += MessegeRecived;
			_userInterface.OnSendMessageRequest += SendMessage;
			_userInterface.OnStartNode += NodeStart;
            _userInterface.OnStopNode += NodeStop;
            _userInterface.OnConnectToNodeRequest += ConnectToNode;
			_userInterface.OnCreateGroupRequest += CreateGroup;

		}

		protected void ConnectToNode(string ip, int port)
		{
			_communication.ConnectToNode(ip, port).Wait();
		}

		protected void NodeStart(int port)
		{
			_communication.StartAsync(port, CancellationToken.None);
		}

        protected void NodeStop(CancellationToken ct)
        {
            _communication.StartAsync(0, ct);
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
			_communication.SendMessage(node, message);
		}

		protected void MessegeRecived(string node, string message)
		{
			_userInterface.ShowMessage(node, message);
		}

        protected void CreateGroup(string groupName)
        {
			_nodeGroups.AddGroup(groupName);
        }

        protected void AddNodeToGroup(string groupName, string node)
        {
            _nodeGroups.AddNodeToGroup(groupName, node);
        }

    }
}
