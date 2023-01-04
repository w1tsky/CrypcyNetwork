using System.Net;

namespace Crypcy.ApplicationCore.Contracts
{
	public interface ICommunication
	{
		IReadOnlyCollection<string> ConnectedNodes { get; }
		event Action<string> OnNodeConnected;
		event Action<string> OnNodeDisconnected;
		event Action<string, string> OnNewMessageRecived;

		Task StartAsync(int port = 0, CancellationToken ct = default);
        Task SendMessageAsync(string node, string message);
        void DropNodeConnection(string node);
		Task ConnectToNode(string ip, int port);
    }
}
