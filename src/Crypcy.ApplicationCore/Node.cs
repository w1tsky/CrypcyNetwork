﻿using Crypcy.ApplicationCore.Contracts;
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

		public Node(IConnectionsManager communication, IMessageSender massageSender, IUserInterface userInterface)
		{
			_connectionsManager = communication;
			_userInterface = userInterface;
			_messageSender = massageSender;

			_connectionsManager.OnNodeConnected += NodeConnected;
			_connectionsManager.OnNodeDisconnected += NodeDisconected;
			_userInterface.OnSendMessageRequest += SendMessage;
			_userInterface.OnStartNode += NodeStart;
			_userInterface.OnConnectToNodeRequest += ConnectToNode;
		}

		protected void ConnectToNode(string ip, int port)
		{
			_connectionsManager.ConnectToNode(ip, port).Wait();
		}

		protected void NodeStart(int port)
		{
			_connectionsManager.StartAsync(port, CancellationToken.None);
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
