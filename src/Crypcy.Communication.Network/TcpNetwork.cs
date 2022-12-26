using Crypcy.ApplicationCore.Contracts;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Crypcy.Communication.Network
{
    public class TcpNetwork : ICommunication
    {
        protected ConcurrentDictionary<string, Socket> _nodes = new ConcurrentDictionary<string, Socket>();

        public IReadOnlyCollection<string> ConnectedNodes => _nodes.Keys.ToList().AsReadOnly();

        public event Action<string> OnNodeConnected = (_) => { };
        public event Action<string> OnNodeDisconnected = (_) => { };
        public event Action<string, string> OnNewMessageRecived = (_,_) => { };

        public void DropNodeConnection(string node)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string node, string message)
        {
            _nodes[node].Send(Encoding.ASCII.GetBytes(message));
        }

        public void Start(int port)
        {

            var connectionHandlerCancellationToken = new CancellationToken();
            Task.Run(() =>
            {
                using var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                tcpSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                tcpSocket.Listen(100);

                while (true)
                {
                    var client = tcpSocket.Accept();
                    NewClientHandle(client);
                }
            }, connectionHandlerCancellationToken);
        }

        protected void NewClientHandle(Socket client) 
        {
            var index = Guid.NewGuid().ToString();
            _nodes[index] = client;

            Task.Run(() => {
                while (true)
                {
                    var buff = new byte[4096];
                    var count = client.Receive(buff);
                    if (count == 0)
                    {
                        client.Close();
                        _nodes.Remove(index,out var _);
                        OnNodeDisconnected.Invoke(index);
                        break;
                    }
                    OnNewMessageRecived.Invoke(index, Encoding.ASCII.GetString(buff, 0, count));
                }
            });

            OnNodeConnected.Invoke(index);
        }
    }
}
