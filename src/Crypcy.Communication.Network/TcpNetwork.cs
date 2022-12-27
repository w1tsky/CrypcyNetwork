using Crypcy.ApplicationCore.Contracts;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Crypcy.Communication.Network
{
	public class TcpNetwork : ICommunication, IDisposable
	{
		protected ConcurrentDictionary<string, TcpClient> _nodes = new ();
		public IReadOnlyCollection<string> ConnectedNodes => _nodes.Keys.ToList().AsReadOnly();

		private IPEndPoint _endPoint;

		public event Action<string> OnNodeConnected = (_) => { };
		public event Action<string> OnNodeDisconnected = (_) => { };
		public event Action<string, string> OnNewMessageRecived = (_, _) => { };

		public void DropNodeConnection(string node)
		{
			throw new NotImplementedException();
		}

		public async Task ConnectToNode(string ip, int port) 
		{
			var client = new TcpClient();
			client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			client.Client.Bind(_endPoint);
			await client.ConnectAsync(IPAddress.Parse(ip), port);

			NewClientHandleAsync(client);
		}

		public void SendMessage(string node, string message)
		{
			var data = Encoding.ASCII.GetBytes(message);
			var size = BitConverter.GetBytes(data.Length);
			_nodes[node].GetStream().WriteAsync(size.Concat(data).ToArray());
		}

		public async Task StartAsync(int port, CancellationToken ct = default)
		{
			_endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
			var tcpListener = new TcpListener(_endPoint);
			tcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			tcpListener.Start(100);
			while (!ct.IsCancellationRequested)
				NewClientHandleAsync(await tcpListener.AcceptTcpClientAsync(ct), ct);
			tcpListener.Stop();
		}

		protected async Task NewClientHandleAsync(TcpClient client, CancellationToken ct = default)
		{
			try
			{
				var index = Guid.NewGuid().ToString();
				_nodes[index] = client;
				OnNodeConnected.Invoke(index);

				void closeConnectionIfRecived0bytes(TcpClient client, int readed)
				{
					if (readed != 0) return;

					client.Close();
					_nodes.Remove(index, out var _);
					OnNodeDisconnected.Invoke(index);
					throw new Exception("Disconnected");
				}

				var stream = client.GetStream();
				while (!ct.IsCancellationRequested)
				{
					var buff4size = new byte[4];
					closeConnectionIfRecived0bytes(client, await stream.ReadAsync(buff4size, 0, 4, ct));
					var size = BitConverter.ToInt32(buff4size);

					var buff4data = new byte[size];
					closeConnectionIfRecived0bytes(client, await stream.ReadAsync(buff4data, 0, size, ct));

					OnNewMessageRecived.Invoke(index, Encoding.ASCII.GetString(buff4data, 0, size));
				}
			}
			finally
			{
				if (client.Connected) client.Close();
			}
		}

		public void Dispose()
		{
			foreach (var n in _nodes) 
			{
				if (n.Value.Connected)
					n.Value.Close();
				n.Value.Dispose();
			}
		}
	}
}
