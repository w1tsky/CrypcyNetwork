using Crypcy.Network.PeerItems;
using Crypcy.Network.PeerNetwork.Old;
using Crypcy.Network.PeerNetwork.PeerTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.Peer
{
    public class NewPeer
    {
        PeerTcpClient PeerTcpClient;
        PeerTcpListener PeerTcpListener;

        private int BUFFER_LENGHT = 4096;

        public List<TcpClient> PeersConnected = new List<TcpClient>();

        public delegate void PacketHandler(Packet peerPacket);
        public event PacketHandler PeerPacketReceived;

        public event EventHandler<string> OnResultsUpdate;

        private bool peerStarted;
        public bool PeerStarted
        {
            get { return peerStarted; }
        }

        public NewPeer(IPEndPoint localPeerEndPoint)
        {
            PeerTcpClient = new PeerTcpClient(localPeerEndPoint, BUFFER_LENGHT);
            PeerTcpListener = new PeerTcpListener(localPeerEndPoint, BUFFER_LENGHT);


            PeerTcpListener.OnResultsUpdate += (sender, result) => OnResultsUpdate?.Invoke(this, result);
            PeerTcpListener.TcpPacketReceived += (packet) => PeerPacketReceived?.Invoke(packet);
            PeerTcpListener.TcpListenerConnections.TcpConnected += (tcpClient) => PeersConnected.Add(tcpClient);
            PeerTcpListener.TcpListenerConnections.TcpDisconnected += (tcpClient) => PeersConnected.Remove(tcpClient);


            PeerTcpClient.OnResultsUpdate += (sender, result) => OnResultsUpdate?.Invoke(this, result);
            PeerTcpClient.TcpPacketReceived += (packet) => PeerPacketReceived?.Invoke(packet);
            PeerTcpClient.TcpClientConnections.TcpConnected += (tcpClient) => PeersConnected.Add(tcpClient);
            PeerTcpClient.TcpClientConnections.TcpDisconnected += (tcpClient) => PeersConnected.Remove(tcpClient);


        }


        public void StartPeer()
        {
            PeerTcpListener.TcpStartListen();
        }

        public void StopPeer()
        {
            OnResultsUpdate?.Invoke(this, "Stopping peer....");
            PeerTcpClient.TcpCloseConnections();
            PeerTcpListener.TcpStopListen();
        }

        public void ConnectToPeer(IPEndPoint remotePeerEndpoint)
        {
            PeerTcpClient.TcpClientConnections.TcpConnect(remotePeerEndpoint);
        }

        public void SendTcpPacketToPeer(IPEndPoint remoteEndpoint, Packet _packet)
        {

            TcpClient tcpClient = PeersConnected.Find(c => c.Client.RemoteEndPoint == remoteEndpoint);

            try
            {

                if (OnResultsUpdate != null)
                    OnResultsUpdate.Invoke(this, $"Sending message to Peer - {tcpClient.Client.RemoteEndPoint.ToString()} - ");

                if (tcpClient.Client != null && tcpClient.Client.Connected)
                {
                    tcpClient.GetStream().BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }

            }
            catch (Exception _ex)
            {
                OnResultsUpdate?.Invoke(this, $"Error sending data to peer {tcpClient.Client.RemoteEndPoint.ToString} via TCP: {_ex}");
            }
        }



    }


}
