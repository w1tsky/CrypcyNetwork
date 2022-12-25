using Crypcy.Network.PeerItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.PeerTcp
{
    public class PeerTcpListener
    {

        public TcpListener TcpListener { get; set; }

        public List<TcpClient> TcpConnectedClients = new List<TcpClient>();

        public event EventHandler<string> OnResultsUpdate;

        public delegate void TcpPacketHandler(Packet packet);
        public event TcpPacketHandler TcpPacketReceived;

        public PeerTcpListnerConnections TcpListenerConnections;

        public PeerTcpListener(IPEndPoint localEndPoint, int tcpBufferLenght)
        {
            TcpListener = new TcpListener(localEndPoint);

            TcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            TcpListener.Server.ExclusiveAddressUse = false;

            TcpListener.Server.ReceiveBufferSize = tcpBufferLenght;
            TcpListener.Server.SendBufferSize = tcpBufferLenght;

            TcpListenerConnections = new PeerTcpListnerConnections(TcpListener, tcpBufferLenght);

            TcpListenerConnections.TcpPacketReceived += (packet) => TcpPacketReceived?.Invoke(packet);
            TcpListenerConnections.OnResultsUpdate += (sender, result) => OnResultsUpdate?.Invoke(this, result);

            TcpListenerConnections.TcpConnected += (tcpClient) => TcpConnectedClients.Add(tcpClient);
            TcpListenerConnections.TcpDisconnected += (tcpClient) => TcpConnectedClients.Remove(tcpClient);

        }

        public void TcpStartListen()
        {
            TcpListener.Start();
            TcpListenerConnections.TcpListen(TcpListener);
            OnResultsUpdate?.Invoke(this, $"Peer Listner started: {TcpListener.Server.LocalEndPoint}");
        }

        public void TcpStopListen()
        {
            foreach(TcpClient tcpClient in TcpConnectedClients)
            {
                tcpClient.Client.Shutdown(SocketShutdown.Both);
                tcpClient.Close();
            }

            if (TcpListener.Server.Connected)
            {
               TcpListener.Server.Shutdown(SocketShutdown.Both);
            }
            
            TcpListener.Stop();
        }

    }
}
