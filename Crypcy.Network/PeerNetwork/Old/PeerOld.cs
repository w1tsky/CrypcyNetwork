using Crypcy.Network.PeerItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.Old
{
    public class OldPeer
    {
        public IPEndPoint LocalEndpoint;

        PeerClientTCP PeerClient;
        PeerListnerTCP PeerListner;

        public delegate void PacketHandler(Packet peerPacket);

        public event PacketHandler PeerPacketReceived;

        public event EventHandler<string> OnResultsUpdate;

        public List<TcpClient> PeersConnected = new List<TcpClient>();

        private bool peerStarted;
        public bool PeerStarted
        {
            get { return peerStarted; }
        }

        public OldPeer(IPEndPoint localPeerEndpoint)
        {
            LocalEndpoint = localPeerEndpoint;
            PeerClient = new PeerClientTCP(LocalEndpoint);
            PeerListner = new PeerListnerTCP(LocalEndpoint);
        }

        public void ReceivePacket(Packet packet)
        {
            PeerPacketReceived.Invoke(packet);
        }

        public void PeerConnected(TcpClient tcpClient)
        {
            PeersConnected.Add(tcpClient);
        }

        public void PeerDisconnected(TcpClient tcpClient)
        {
            PeersConnected.Remove(tcpClient);
        }

        public void StartPeer()
        {
            PeerListner.StartListen();

            // Handler PeerListener Connections
            PeerListner.PeerConnected += PeerConnected;
            PeerListner.PeerDisconnected += PeerDisconnected;


            // Handle PeerClient Connections
            PeerClient.ConnectedToPeer += PeerConnected;
            PeerClient.DisconnectedFromPeer += PeerDisconnected;

            // Handle Peer Packaets
            PeerListner.PeerListenerPacketReceived += ReceivePacket;
            PeerClient.PeerClientPacketReceived += ReceivePacket;

            peerStarted = true;
        }

        public void StopPeer()
        {
            PeersConnected.Clear();

            PeerListner.StopListen();
            PeerClient.StopListen();


            peerStarted = false;
        }

        public void ConnectToPeer(IPEndPoint remotePeerEndpoint)
        {
            PeerClient.PeerConnect(remotePeerEndpoint);
        }


        public void SendPacketToPeer(IPEndPoint remoteEndpoint, Packet _packet)
        {

            var tcpClient = PeersConnected.Find(c => c.Client.RemoteEndPoint == remoteEndpoint);

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
                if (OnResultsUpdate != null)
                    OnResultsUpdate.Invoke(this, $"Error sending data to peer {tcpClient.Client.RemoteEndPoint.ToString} via TCP: {_ex}");
            }
        }



    }


}
