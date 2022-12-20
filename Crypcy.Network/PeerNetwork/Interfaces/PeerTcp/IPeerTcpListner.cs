using Crypcy.Network.PeerItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.Interfaces.PeerTcp
{
    public interface IPeerTcpListner
    {
        public IPEndPoint LocalEndpoint { get; set; }

        public TcpListener ListnerTCP { get; set; }
        public byte[] BufferTCP { get; set; }

        public List<TcpClient> tcpClients { get; set; }

        public delegate void PacketHandler(Packet _packet);
        public event PacketHandler PeerListenerPacketReceived;

        public delegate void PeersHandler(TcpClient tcpClient);
        public event PeersHandler PeerConnected;
        public event PeersHandler PeerDisconnected;


        
    }
}
