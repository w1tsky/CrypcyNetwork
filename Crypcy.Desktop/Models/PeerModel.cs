using Crypcy.Network.PeerNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Desktop.Models
{
    public class PeerModel
    {
        IPEndPoint LocalEndpoint { get; set; }
        Peer LocalPeer { get; set; }

        public PeerModel(IPEndPoint localEndpoint)
        {
            LocalEndpoint = localEndpoint;
            LocalPeer = new Peer(LocalEndpoint);
        }
    }
}
