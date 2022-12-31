using Crypcy.Network.PeerItems;
using Crypcy.Network.PeerNetwork.Interfaces.PeerTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.PeerTcp
{
    public class PeerTcpClientConnections
    {

        public IPEndPoint LocalEndPoint;
        TcpClient TcpClient;
        byte[] TcpBuffer;
        public PeerTcpReceiveHandler TcpReceiveHandler { get; set; }

        public delegate void TcpConnectionHandler(TcpClient tcpClient);
        public event TcpConnectionHandler TcpConnected;
        public event TcpConnectionHandler TcpDisconnected;

        public delegate void TcpPacketHandler(Packet packet);
        public event TcpPacketHandler TcpPacketReceived;

        public event EventHandler<string> OnResultsUpdate;

        public PeerTcpClientConnections(TcpClient tcpClient, int tcpBufferLenght)
        {
            TcpBuffer = new byte[tcpBufferLenght];

            TcpClient = tcpClient;
            LocalEndPoint = (IPEndPoint?)TcpClient.Client.LocalEndPoint;



            TcpReceiveHandler = new PeerTcpReceiveHandler(TcpBuffer);
            TcpReceiveHandler.OnResultsUpdate += (sender, result) => OnResultsUpdate?.Invoke(this, result);
            TcpReceiveHandler.TcpPacketReceived += (packet) => TcpPacketReceived?.Invoke(packet);
            TcpReceiveHandler.TcpDisconnected += (tcpClient) => TcpDisconnected?.Invoke(tcpClient);
        }

        public void TcpConnect(IPEndPoint connectEndpoint)
        {
            TcpClient tcpClient = new TcpClient();

            tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            tcpClient.Client.ExclusiveAddressUse = false;
            tcpClient.Client.Bind(LocalEndPoint);

            tcpClient.SendBufferSize = TcpBuffer.Length;
            tcpClient.ReceiveBufferSize = TcpBuffer.Length;

            try
            {
                tcpClient.BeginConnect(connectEndpoint.Address, connectEndpoint.Port, TcpConnectHandle, TcpClient);
            }
            catch (Exception ex)
            {
                OnResultsUpdate?.Invoke(this, $"Unnable connect to peer with endpoint {connectEndpoint.ToString}: {ex}");
            }
        }

        public void TcpConnectHandle(IAsyncResult asyncResult)
        {
            TcpClient? tcpClient = asyncResult.AsyncState as TcpClient;

            try
            {
                tcpClient?.EndConnect(asyncResult);
                TcpConnected?.Invoke(tcpClient);

                tcpClient.GetStream().BeginRead(TcpBuffer, 0, TcpBuffer.Length, TcpReceiveHandler.TcpReceiveHandler, tcpClient);

                OnResultsUpdate?.Invoke(this, $"Succecefully conneted to peer {tcpClient.Client.RemoteEndPoint}");
            }
            catch (Exception ex)
            {
                OnResultsUpdate?.Invoke(this, $"Unnable connect to peer {tcpClient.Client.RemoteEndPoint}: {ex}");
            }
        }



    }
}
