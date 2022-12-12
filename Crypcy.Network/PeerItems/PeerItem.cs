using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerItems
{
    public enum PeerItemType
    {
        PeerInfo,
        Message,
        Req,
        Notification,
        Ack,
        KeepAlive
    }
    public class PeerItem
    {
        public long ID { get; set; }
        public PeerItemType PeerItemType { get; set; }
    }
}
