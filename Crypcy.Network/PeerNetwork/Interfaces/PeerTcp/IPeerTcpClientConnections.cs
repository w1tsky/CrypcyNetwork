using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.Interfaces.PeerTcp
{
    public interface IPeerTcpClientConnections
    {
        public Thread TcpConnectionThreads { get; set; } 
        public List<Thread> TcpConnectionsThreads { get; set; }

        public List<TcpClient> tcpClients { get; set; }
    }
}
