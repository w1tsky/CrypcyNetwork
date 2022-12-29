using Crypcy.Network.PeerItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Crypcy.Network.PeerNetwork.Old
{
    public class PeerClientTCP
    {
        private IPEndPoint LocalEndpoint;

        public TcpClient ClientTCP;
        private Packet PacketTCP;
        private byte[] BufferTCP;


        public delegate void PacketHandler(Packet peerPacket);
        public event PacketHandler PeerClientPacketReceived;

        public delegate void PeertHandler(TcpClient tcpClient);

        public event PeertHandler ConnectedToPeer;
        public event PeertHandler DisconnectedFromPeer;

        private List<TcpClient> tcpClients = new List<TcpClient>();


        public PeerClientTCP(IPEndPoint localEndpoint)
        {
            LocalEndpoint = localEndpoint;
        }

        public void StartListen(TcpClient tcpClient)
        {
            BufferTCP = new byte[4096];
            tcpClient.GetStream().BeginRead(BufferTCP, 0, BufferTCP.Length, ReceiveClientCallback, tcpClient);
        }

        public void StopListen()
        {
            foreach (TcpClient tcpClient in tcpClients)
            {
                tcpClient.Close();

            }

        }


        public void PeerConnect(IPEndPoint peerConnectEndpoint)
        {

            try
            {
                BufferTCP = new byte[4096];

                //Init Peer Tcp Client
                ClientTCP = new TcpClient();

                ClientTCP.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                ClientTCP.Client.ExclusiveAddressUse = false;
                ClientTCP.Client.Bind(LocalEndpoint);

                ClientTCP.SendBufferSize = 4096;
                ClientTCP.ReceiveBufferSize = 4096;


                ClientTCP.BeginConnect(peerConnectEndpoint.Address, peerConnectEndpoint.Port, ConnectCallback, ClientTCP);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private void ConnectCallback(IAsyncResult asyncResult)
        {
            TcpClient? tcpClient = asyncResult.AsyncState as TcpClient;

            try
            {
                tcpClient?.EndConnect(asyncResult);
                StartListen(tcpClient);
                tcpClients.Add(tcpClient);
                ConnectedToPeer.Invoke(tcpClient);

                Console.WriteLine($"Succecefully conneted to peer {tcpClient.Client.RemoteEndPoint}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void ReceiveClientCallback(IAsyncResult asyncResult)
        {

            using (TcpClient? tcpClient = asyncResult.AsyncState as TcpClient)
            {
                PacketTCP = new Packet();

                if (tcpClient == null || tcpClient.Client == null)
                {
                    // handle null TcpClient object
                    return;
                }

                try
                {
                    int receivedByteLength = tcpClient.Client.EndReceive(asyncResult);
                    if (receivedByteLength <= 0)
                    {
                        Console.WriteLine($"Peer disconnected: {tcpClient?.Client?.RemoteEndPoint?.ToString()}");

                        tcpClient?.Close();
                        tcpClients.Remove(tcpClient);
                        DisconnectedFromPeer?.Invoke(tcpClient);

                        return;
                    }

                    byte[] receivedData = new byte[receivedByteLength];
                    Array.Copy(BufferTCP, receivedData, receivedByteLength);
                    PacketTCP.Reset(HandleReceviedData(receivedData));
                    tcpClient.Client.BeginReceive(BufferTCP, 0, BufferTCP.Length, SocketFlags.None, ReceiveClientCallback, tcpClient);
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error receiving TCP data: {_ex.Message}");
                    Console.WriteLine($"Peer disconnected: {tcpClient?.Client.RemoteEndPoint.ToString()}");

                    tcpClient?.Close();
                    tcpClients.Remove(tcpClient);
                    DisconnectedFromPeer?.Invoke(tcpClient);

                }
            }


        }

        private bool HandleReceviedData(byte[] receivedData)
        {
            int _packetLength = 0;

            PacketTCP.SetBytes(receivedData);

            if (PacketTCP.UnreadLength() >= 4)
            {
                _packetLength = PacketTCP.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= PacketTCP.UnreadLength())
            {
                byte[] _packetBytes = PacketTCP.ReadBytes(_packetLength);

                using (Packet _packet = new Packet(_packetBytes))
                {
                    Console.WriteLine($"PeerClient - Packet received: {_packet.ToString()}");
                    PeerClientPacketReceived?.Invoke(_packet);
                }


                _packetLength = 0;

                if (PacketTCP.UnreadLength() >= 4)
                {
                    _packetLength = PacketTCP.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }

    }
}
