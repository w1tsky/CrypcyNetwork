using Crypcy.Network.PeerItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.PeerTcp
{
    public class PeerTcpPacketHandler : IPeerTcpPacketHandler
    {
        public event EventHandler<string> OnResultsUpdate;

        public void ReceivePacket(Packet packet)
        {
            throw new NotImplementedException();
        }

        public void SendPacketToPeer(TcpClient tcpClient, IPEndPoint remoteEndpoint, Packet packet)
        {
            throw new NotImplementedException();
        }
    }
}
