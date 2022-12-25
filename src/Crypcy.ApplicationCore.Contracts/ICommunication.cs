namespace Crypcy.ApplicationCore.Contracts
{
    public interface ICommunication
    {
        public IReadOnlyCollection<string> ConnectedNodes { get; }
        public event Action<string> OnNodeConnected;
        public event Action<string> OnNodeDisconnected;
        public event Action<string, string> OnNewMessageRecived;

        public void SendMessage(string node, string message);
        public void DropNodeConnection(string node);
    }
}
