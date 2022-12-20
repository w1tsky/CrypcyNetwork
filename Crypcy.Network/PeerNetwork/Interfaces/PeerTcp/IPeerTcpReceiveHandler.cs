using Crypcy.Network.PeerItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.Interfaces.PeerTcp
{
    public interface IPeerTcpReceiveHandler
    {
        public void ReceiveHandler(IAsyncResult asyncResult);
        public bool ReceivedDataHandler(byte[] receivedData);
    }
}
