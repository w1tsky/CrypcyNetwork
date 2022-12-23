using Crypcy.Network.PeerItems;
using Crypcy.Network.PeerNetwork.Interfaces.PeerTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Crypcy.Network.PeerNetwork.Peer;

namespace Crypcy.Network.PeerNetwork.PeerTcp
{
    public class PeerTcpClient
    {
        
        
        byte[] TcpBuffer;
        public TcpClient TcpClient { get; set; }
        public List<TcpClient> TcpClients= new List<TcpClient>();

        public PeerTcpClientConnections TcpClientConnections;

        public event EventHandler<string> OnResultsUpdate;

        public delegate void TcpPacketHandler(Packet packet);
        public event TcpPacketHandler TcpPacketReceived;

        public PeerTcpClient(IPEndPoint localEndPoint, int tcpBufferLenght)
        {
            TcpBuffer = new byte[tcpBufferLenght];
            TcpClient = new TcpClient();

            TcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            TcpClient.Client.ExclusiveAddressUse = false;
            TcpClient.Client.Bind(localEndPoint);

            TcpClient.SendBufferSize = TcpBuffer.Length;
            TcpClient.ReceiveBufferSize = TcpBuffer.Length;

            TcpClientConnections = new PeerTcpClientConnections(TcpBuffer, TcpClient);

            TcpClientConnections.TcpReceiveHandler.TcpPacketReceived += (packet) => TcpPacketReceived?.Invoke(packet);
            TcpClientConnections.OnResultsUpdate += (sender, result) => OnResultsUpdate?.Invoke(this, result);

            TcpClientConnections.TcpConnected += (tcpClient) => TcpClients.Add(tcpClient);
            TcpClientConnections.TcpDisconnected += (tcpClient) => TcpClients.Remove(tcpClient);
        }

        public void TcpCloseConnections()
        {
            foreach(TcpClient tcpClient in TcpClients)
            {
                tcpClient.Client.Shutdown(SocketShutdown.Both);
                tcpClient.Close();  
            }
        }


    }
}
