using Crypcy.Network.PeerNetwork.Interfaces.PeerTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.PeerTcp
{
    public class PeerTcpListnerConnections : IPeerTcpListnerHandler
    {
        public void TcpIncomingConnectionHandle(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        public void TcpDisconnectionHandle(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }
    }
}
