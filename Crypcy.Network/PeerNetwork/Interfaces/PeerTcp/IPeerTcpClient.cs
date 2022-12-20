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
    public interface IPeerTcpClient
    {

        public IPEndPoint LocalEndpoint { get; set; }
        public TcpClient ClientTCP { get; set; }
        public byte[] BufferTCP { get; set; }

        public event EventHandler<string> OnResultsUpdate;

        public delegate void PacketHandler(Packet peerPacket);
        public event PacketHandler PeerClientPacketReceived
        {
            add { }
            remove { }
        }

        public delegate void PeerHandler(TcpClient tcpClient);

        public event PeerHandler ConnectedToPeer 
        {
            add { } 
            remove { } 
        }
        public event PeerHandler DisconnectedFromPeer
        {
            add { }
            remove { } 
        }

        

        public void HandleTcpConnection(TcpClient tcpClient);
        public void StopHandleTcpConnection();



    }
}
