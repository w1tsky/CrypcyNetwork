using Crypcy.Network.PeerItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.Peer
{
    public class PeerPacketHandler : IPeerTcpPacketHandler
    {
        public delegate void PacketHandler(Packet peerPacket);
        public event PacketHandler PeerClientPacketReceived;

        public event EventHandler<string> OnResultsUpdate;

        public void ReceivePacket(Packet packet)
        {
            PeerClientPacketReceived?.Invoke(packet);
        }

        public void SendPacketToPeer(TcpClient tcpClient, IPEndPoint remoteEndpoint, Packet packet)
        {
            try
            {

                if (tcpClient.Client != null && tcpClient.Client.Connected)
                {
                    tcpClient.GetStream().BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);

                    if (OnResultsUpdate != null)
                        OnResultsUpdate.Invoke(this, $"Sending message to Peer - {tcpClient.Client.RemoteEndPoint.ToString()} - ");
                }

            }
            catch (Exception _ex)
            {
                if (OnResultsUpdate != null)
                    OnResultsUpdate.Invoke(this, $"Error sending data to peer {tcpClient.Client.RemoteEndPoint.ToString} via TCP: {_ex}");
            }
        }
    }
}


