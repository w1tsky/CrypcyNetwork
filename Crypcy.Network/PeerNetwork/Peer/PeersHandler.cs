using Crypcy.Network.PeerItems;
using Crypcy.Network.PeerNetwork.PeerTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.Peer
{
    public class PeersHandler : IPeersHandler
    {
    
        public List<TcpClient> PeersConnected { get; set; }

        public PeersHandler()
        {
            PeersConnected = new List<TcpClient>();

        }

        public void PeerConnected(TcpClient tcpClient)
        {
            PeersConnected.Add(tcpClient);
        }

        public void PeerDisconnected(TcpClient tcpClient)
        {
            PeersConnected.Remove(tcpClient);
        }


    }


}
