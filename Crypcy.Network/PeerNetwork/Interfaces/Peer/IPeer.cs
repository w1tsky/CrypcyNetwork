using System.Net;
using System.Net.Sockets;
using Crypcy.Network.PeerItems;

public interface IPeer
{
    public delegate void PacketHandler(Packet peerPacket);

    public event PacketHandler PeerPacketReceived;

    public event EventHandler<string> OnResultsUpdate;

    public List<TcpClient> PeersConnected { get; }

    public bool PeerStarted { get; }

    public void ReceivePacket(Packet packet);

    public void PeerConnected(TcpClient tcpClient);

    public void PeerDisconnected(TcpClient tcpClient);

    public void StartPeer();

    public void StopPeer();

    public void ConnectToPeer(IPEndPoint remotePeerEndpoint);

    public void SendPacketToPeer(IPEndPoint remoteEndpoint, Packet packet);
}
