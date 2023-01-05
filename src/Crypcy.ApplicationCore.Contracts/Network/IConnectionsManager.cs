using System.Net;

namespace Crypcy.ApplicationCore.Contracts.Network
{
	public interface IConnectionsManager
	{
		IReadOnlyCollection<string> ConnectedNodes { get; }
		event Action<string> OnNodeConnected;
		event Action<string> OnNodeDisconnected;

		Task StartAsync(int port, CancellationToken ct);
		void DropNodeConnection(string node);
		Task ConnectToNode(string ip, int port);
	}
}
