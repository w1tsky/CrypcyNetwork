using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.Interfaces.PeerTcp
{
    public interface IPeerTcpClientConnect
    {

        public delegate void PeerHandler(TcpClient tcpClient);

        public event PeerHandler ConnectedToPeer { add { } remove { } }
        public void TcpConnect(IPEndPoint connectEndpoint);
        public void TcpConnectHandle(IAsyncResult asyncResult);
    }
}
