using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore
{
    public enum PacketType
    {
        NodeInfo,
        Message,
        Req,
        Notification,
        Ack,
        KeepAlive
    }

    public class Packet
    {
        public string Id { get; set; }
        public PacketType Type { get; set; }
    }

    public class NodeInfo : Packet
    {
        public string NodeId { get; set; }   
        public string ExternalIp { get; set; }
        public string ExternalPort { get; set; }
    }

    public class Message : Packet
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }

        public Message(string from, string to, string content)
        {
            Type = PacketType.Message;
            From = from;
            To = to;
            Content = content;
        }
    }

}
