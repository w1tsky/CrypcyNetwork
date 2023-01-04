using Crypcy.ApplicationCore.Contracts;
using Crypcy.ApplicationCore.Contracts.Network;
using Crypcy.ApplicationCore.TempMessageSolution;
using Crypcy.ApplicationCore.TempMessageSolution.MessagesTypes;
using System.Net;

namespace Crypcy.ApplicationCore
{
    public class Nodes
	{
		protected HashSet<string> _nodes = new();

		private readonly INodeManager _nodeManager;
		private readonly IUserInterface _userInterface;
		private readonly MessageSender _messageSender;

		public Nodes(INodeManager communication, MessageSender massageSender, IUserInterface userInterface)
		{
			_nodeManager = communication;
			_userInterface = userInterface;
			_messageSender = massageSender;

			_nodeManager.OnNodeConnected += NodeConnected;
			_nodeManager.OnNodeDisconnected += NodeDisconected;
			_userInterface.OnSendMessageRequest += SendMessage;
			_userInterface.OnStartNode += NodeStart;
			_userInterface.OnConnectToNodeRequest += ConnectToNode;
		}

		protected void ConnectToNode(string ip, int port)
		{
			_nodeManager.ConnectToNode(ip, port).Wait();
		}

		protected void NodeStart(int port)
		{
			_nodeManager.StartAsync(port, CancellationToken.None);
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
			_messageSender.SendMessage(node, new TextMessage() { Text = message});
		}

		protected void MessegeRecived(string node, string message)
		{
			_userInterface.ShowMessage(node, message);
		}

	}
}
