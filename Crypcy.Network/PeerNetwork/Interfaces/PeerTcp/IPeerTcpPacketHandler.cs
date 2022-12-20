using System.Net;
using System.Net.Sockets;
using Crypcy.Network.PeerItems;

namespace Crypcy.Network.PeerNetwork.Interfaces.PeerTcp
{
    public interface IPeerTcpPacketHandler
    {
        public delegate void PacketHandler(Packet peerPacket);
        public event PacketHandler PeerClientPacketReceived
        {
            add { }
            remove { }
        }

        public event EventHandler<string> OnResultsUpdate;

    }
}