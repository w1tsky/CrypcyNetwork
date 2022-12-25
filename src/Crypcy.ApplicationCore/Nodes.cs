using Crypcy.ApplicationCore.Contracts;

namespace Crypcy.ApplicationCore
{
    public class Nodes
    {
        protected HashSet<string> _nodes = new HashSet<string>();

        private readonly ICommunication _communication;
        private readonly IUserInterface _console;

        public Nodes(ICommunication communication, IUserInterface console)
        {
            _communication = communication;
            _console = console;

            _communication.OnNodeConnected += NodeConnected;
            _communication.OnNewMessageRecived += MessegeRecived;
            _console.OnSendMessageRequest += SendMessage;
            _communication.OnNodeDisconnected += NodeDisconected;
        }

        protected void NodeConnected(string node) 
        {
            lock (_nodes) _nodes.Add(node);   
            _console.NewNodeConnectedNotification(node);
        }

        protected void MessegeRecived(string node, string message) 
        {
            _console.ShowMessage(node, message);
        }

        protected void SendMessage(string node, string message) 
        {
            _communication.SendMessage(node, message);
        }

        protected void NodeDisconected(string node) 
        {
            lock (_nodes) _nodes.Remove(node);
            _console.NodeDiconnectedNotification(node);
        }
    }
}
