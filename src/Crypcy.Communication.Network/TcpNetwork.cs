using Crypcy.ApplicationCore.Contracts.Network;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Crypcy.Communication.Network
{
    internal sealed class TcpNetwork : IConnectionsManager, IReciveData, ISendData, IDisposable
	{
		private ConcurrentDictionary<string, TcpClient> _nodes = new ();
		private IPEndPoint? _endPoint;
        public IReadOnlyCollection<string> ConnectedNodes => _nodes.Keys.ToList().AsReadOnly();

        public event Action<string> OnNodeConnected = (_) => { };
		public event Action<string> OnNodeDisconnected = (_) => { };
		public event Action<string, byte[]> OnNewMessageRecived = (_, _) => { };

		public async Task StartAsync(int port = 0, CancellationToken ct = default)
		{
			_endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
			var tcpListener = new TcpListener(_endPoint);
			tcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			tcpListener.Start(100);
			while (!ct.IsCancellationRequested)
				NewClientHandleAsync(await tcpListener.AcceptTcpClientAsync(ct), ct);
			tcpListener.Stop();
		}
		public async Task ConnectToNode(string ip, int port)
		{
			var client = new TcpClient();
			client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			client.Client.Bind(_endPoint);
			await client.ConnectAsync(IPAddress.Parse(ip), port);

            await NewClientHandleAsync(client);
		}
		public void DropNodeConnection(string node)
		{
			_nodes[node].Close();
			_nodes.Remove(node, out var _);
			OnNodeDisconnected.Invoke(node);
		}

		public ValueTask SendMessageAsync(string node, byte[] message)
		{
			var size = BitConverter.GetBytes(message.Length);
			return _nodes[node].GetStream().WriteAsync(size.Concat(message).ToArray());
		}

		private async Task NewClientHandleAsync(TcpClient client, CancellationToken ct = default)
		{
			var index = Guid.NewGuid().ToString();
			try
			{
				_nodes[index] = client;
				OnNodeConnected.Invoke(index);

				void closeConnectionIfRecived0bytes(TcpClient client, int readed)
				{
					if (readed == 0) throw new Exception("Disconnected");
				}

				var stream = client.GetStream();
				while (!ct.IsCancellationRequested)
				{
					var buff4size = new byte[4];
					closeConnectionIfRecived0bytes(client, await stream.ReadAsync(buff4size, 0, 4, ct));
					var size = BitConverter.ToInt32(buff4size);

					var buff4data = new byte[size];
					closeConnectionIfRecived0bytes(client, await stream.ReadAsync(buff4data, 0, size, ct));

					OnNewMessageRecived.Invoke(index, buff4data);
				}
			}
			finally
			{
				DropNodeConnection(index);
			}
		}

		public void Dispose()
		{
			foreach (var n in _nodes)
				if (n.Value.Connected)
				{
					n.Value.Close();
					OnNodeDisconnected.Invoke(n.Key);
				}

			_nodes.Clear();
		}

    }
}
