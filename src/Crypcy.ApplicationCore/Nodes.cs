using Crypcy.ApplicationCore.Contracts;
using System.Net;

namespace Crypcy.ApplicationCore
{
	public class Nodes
	{
		protected HashSet<string> _nodes = new HashSet<string>();

		private readonly ICommunication _communication;
		private readonly IUserInterface _userInterface;

		public Nodes(ICommunication communication, IUserInterface userInterface)
		{
			_communication = communication;
			_userInterface = userInterface;

			_communication.OnNodeConnected += NodeConnected;
			_communication.OnNodeDisconnected += NodeDisconected;
			_communication.OnNewMessageRecived += MessegeRecived;
			_userInterface.OnSendMessageRequest += SendMessage;
			_userInterface.OnStartNode += NodeStart;
			_userInterface.OnConnectToNodeRequest += ConnectToNode;
		}

		protected void ConnectToNode(string ip, int port)
		{
			_communication.ConnectToNode(ip, port).Wait();
		}

		protected void NodeStart(int port)
		{
			_communication.StartAsync(port, CancellationToken.None);
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

	}
}
