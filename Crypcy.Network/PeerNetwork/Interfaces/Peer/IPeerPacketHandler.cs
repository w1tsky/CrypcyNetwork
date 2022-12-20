using System.Net;
using System.Net.Sockets;
using Crypcy.Network.PeerItems;

public interface IPeerTcpPacketHandler
{
    public delegate void PacketHandler(Packet peerPacket);
    public event PacketHandler PeerClientPacketReceived {
        add { }
        remove { } 
    }

    public event EventHandler<string> OnResultsUpdate;

    public void ReceivePacket(Packet packet);
    public void SendPacketToPeer(TcpClient tcpClient, IPEndPoint remoteEndpoint, Packet packet);
}
