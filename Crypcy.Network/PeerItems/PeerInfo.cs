using Crypcy.Network.PeerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerItems
{
    [Serializable]
    public class PeerInfo : PeerItem
    {
        public string Name { get; set; }
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint InternalEndpoint { get; set; }
        [NonSerialized]
        public TcpClient PeerClientTCP;
        [NonSerialized]
        public bool Initialized;

        public bool Update(PeerInfo peer)
        {
            PeerItemType = PeerItemType.PeerInfo;

            if (ID == peer.ID)
            {
                foreach (PropertyInfo P in peer.GetType().GetProperties())
                    if (P.GetValue(peer) != null)
                        P.SetValue(this, P.GetValue(peer));

            }
            return (ID == peer.ID);
        }
    }
}
