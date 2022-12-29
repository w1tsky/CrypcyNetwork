using System.Net;
using System.Net.Sockets;
using Crypcy.Network.PeerItems;

public interface IPeersHandler
{

    public List<TcpClient> PeersConnected { get; set; }

    public void PeerConnected(TcpClient tcpClient);

    public void PeerDisconnected(TcpClient tcpClient);


}
