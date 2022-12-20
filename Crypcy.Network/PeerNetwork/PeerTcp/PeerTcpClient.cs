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
    public class PeerTcpClient : IPeerTcpClient
    {

        public IPEndPoint LocalEndpoint { get; set; }
        public TcpClient ClientTCP { get; set; }
        public byte[] BufferTCP { get; set; }

       
        public event EventHandler<string> OnResultsUpdate;

        PeerPacketHandler PeerPacketHandler { get; set; }
        public delegate void PeertHandler(TcpClient tcpClient);

        public List<Thread> TcpConnectionsThreads { get; set; }

        public Thread TcpConnectionThread { get; set; }

        public void HandleTcpConnection(TcpClient tcpClient)
        {
            
        }

        public void StopHandleTcpConnection()
        {
           
        }
    }
}
