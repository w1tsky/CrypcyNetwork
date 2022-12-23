using Crypcy.Network.PeerNetwork.Interfaces.PeerTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Crypcy.Network.PeerItems;
using System.Net.Http;

namespace Crypcy.Network.PeerNetwork.PeerTcp
{
    public class PeerTcpListnerConnections
    {

        private TcpListener TcpListener;

        private byte[] TcpListenerBuffer;
        public PeerTcpReceiveHandler TcpReceiveHandler { get; set; }

        public delegate void TcpConnectionHandler(TcpClient tcpClient);
        public event TcpConnectionHandler TcpConnected;
        public event TcpConnectionHandler TcpDisconnected;

        public delegate void TcpPacketHandler(Packet packet);
        public event TcpPacketHandler TcpPacketReceived;

        public event EventHandler<string> OnResultsUpdate;


        public PeerTcpListnerConnections(TcpListener tcpListener, int tcpBufferLenght)
        {
            TcpListener = tcpListener;
            TcpListenerBuffer = new byte[tcpBufferLenght];

            TcpReceiveHandler = new PeerTcpReceiveHandler(TcpListenerBuffer);

            TcpReceiveHandler.TcpPacketReceived += (packet) => TcpPacketReceived?.Invoke(packet);
            TcpReceiveHandler.TcpDisconnected += (tcpClient) => TcpDisconnected?.Invoke(tcpClient);
        }

        public void TcpListen(TcpListener tcpListener)
        {
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectionsHandle), TcpListener);
            OnResultsUpdate?.Invoke(this, $"Peer Listner receving incomming connection on address: {tcpListener.Server.LocalEndPoint.ToString}");
        }



        public void TcpConnectionsHandle(IAsyncResult asyncResult)
        {
            TcpClient tcpClient = TcpListener.EndAcceptTcpClient(asyncResult);

            tcpClient.ReceiveBufferSize = TcpListener.Server.ReceiveBufferSize;
            tcpClient.SendBufferSize = TcpListener.Server.SendBufferSize;

            try
            {

                TcpConnected.Invoke(tcpClient);

                tcpClient.Client.BeginReceive(TcpListenerBuffer, 0, TcpListenerBuffer.Length, SocketFlags.None, TcpReceiveHandler.TcpReceiveHandler, tcpClient);

                OnResultsUpdate?.Invoke(this, $"Peer connected with Endpoint: {tcpClient.Client.RemoteEndPoint.ToString}");


            }
            catch (Exception ex)
            {
                OnResultsUpdate?.Invoke(this, $"Unnable to listen icomming connection {tcpClient.Client.RemoteEndPoint}: {ex.Message}");
            }

        }


    }
}
