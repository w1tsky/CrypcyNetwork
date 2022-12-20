using Crypcy.Network.PeerItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.Interfaces.PeerTcp
{
    public interface IPeerTcpListnerHandler
    {
        public void TcpIncomingConnectionHandle(IAsyncResult asyncResult);
        public void TcpDisconnectionHandle(IAsyncResult asyncResult);
    }

}
