using Crypcy.ApplicationCore.Contracts;
using Crypcy.ApplicationCore.Contracts.Network;
using Crypcy.ApplicationCore.MessageProcessing;
using Crypcy.ApplicationCore.MessagesTypes;

namespace Crypcy.ApplicationCore
{
    public class Node
	{
		protected HashSet<string> _nodes = new();

		private readonly IConnectionsManager _connectionsManager;
		private readonly IUserInterface _userInterface;
		private readonly IMessageSender _messageSender;

        private CancellationToken _token;
        private CancellationTokenSource _cts;

        public Node(IConnectionsManager connections, IMessageSender massageSender, IUserInterface userInterface)
		{
            _cts = new();
            _token = _cts.Token;

            _connectionsManager = connections;
			_userInterface = userInterface;
			_messageSender = massageSender;

			_connectionsManager.OnNodeConnected += NodeConnected;
			_connectionsManager.OnNodeDisconnected += NodeDisconected;
			_userInterface.OnSendMessageRequest += SendMessage;
			_userInterface.OnStartNode += NodeStart;
            _userInterface.OnStopNode += NodeStop;
            _userInterface.OnConnectToNodeRequest += ConnectToNode;
		}

        protected void NodeStart(int port)
        {
            _connectionsManager.StartAsync(port, _token);
        }

        protected void NodeStop()
        {
            _cts.Cancel();
        }

        protected void ConnectToNode(string ip, int port)
        {
            _connectionsManager.ConnectToNode(ip, port).Wait();
        }

        protected void NodeConnected(string node)
		{
			lock (_nodes) 
				_nodes.Add(node);

			_userInterface.NodeConnectedNotification(node);
		}
		protected void NodeDisconected(string node)
		{
			lock (_nodes) 
				_nodes.Remove(node);

			_userInterface.NodeDiconnectedNotification(node);
		}

		protected void SendMessage(string node, string message)
		{
			_messageSender.SendMessageAsync(node, new TextMessage()
			{ 
				Text = message
			}).AsTask().Wait();
		}

		protected void MessegeRecived(string node, string message)
		{
			_userInterface.ShowMessage(node, message);
		}
	}
}
