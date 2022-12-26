using Crypcy.ApplicationCore.Contracts;
using System.Net;

namespace Crypcy.ApplicationCore
{
    public class Nodes
    {
        protected HashSet<string> _nodes = new HashSet<string>();

        private readonly ICommunication _communication;
        private readonly IUserInterface _interactionHandler;

        public Nodes(ICommunication communication, IUserInterface interactionHandler)
        {
            _communication = communication;
            _interactionHandler = interactionHandler;

            _communication.OnNodeConnected += NodeConnected;
            _communication.OnNodeDisconnected += NodeDisconected;
            _communication.OnNewMessageRecived += MessegeRecived;
            _interactionHandler.OnSendMessageRequest += SendMessage;
            _interactionHandler.OnStartNode += NodeStart;


        }

        protected void NodeStart(int port)
        {
            _communication.Start(port);
        }

        protected void NodeConnected(string node) 
        {
            lock (_nodes) _nodes.Add(node);   
            _interactionHandler.NodeConnectedNotification(node);
        }
        protected void NodeDisconected(string node)
        {
            lock (_nodes) _nodes.Remove(node);
            _interactionHandler.NodeDiconnectedNotification(node);
        }

        protected void SendMessage(string node, string message)
        {
            _communication.SendMessage(node, message);
        }

        protected void MessegeRecived(string node, string message) 
        {
            _interactionHandler.ShowMessage(node, message);
        }

    }
}
