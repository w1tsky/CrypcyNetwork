using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Crypcy.Network.PeerItems;

namespace Crypcy.Network.PeerNetwork.Old
{
    public class PeerListnerTCP
    {
        IPEndPoint LocalEndpoint;

        private TcpListener ListnerTCP;
        private Packet PacketTCP;
        private byte[] BufferTCP;

        public delegate void PacketHandler(Packet _packet);
        public event PacketHandler PeerListenerPacketReceived;

        public delegate void PeersHandler(TcpClient tcpClient);
        public event PeersHandler PeerConnected;
        public event PeersHandler PeerDisconnected;


        private List<TcpClient> tcpClients = new List<TcpClient>();

        public PeerListnerTCP(IPEndPoint localEndpoint)
        {
            LocalEndpoint = localEndpoint;
            ListnerTCP = new TcpListener(localEndpoint);
            ListnerTCP.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            ListnerTCP.Server.ExclusiveAddressUse = false;

            ListnerTCP.Server.ReceiveBufferSize = 4096;
            ListnerTCP.Server.SendBufferSize = 4096;

        }

        public void StartListen()
        {
            ListnerTCP.Start();
            ListnerTCP.BeginAcceptTcpClient(new AsyncCallback(AcceptCallback), ListnerTCP);
            Console.WriteLine($"Peer Listner Started on address: {LocalEndpoint.ToString()}");
        }

        public void StopListen()
        {
            foreach (TcpClient tcpClient in tcpClients)
            {
                tcpClient.Close();
                PeerDisconnected.Invoke(tcpClient);
            }

            ListnerTCP.Stop();
            ListnerTCP.Server.Dispose();
            Console.WriteLine($"Peer Listner Stopped");
        }

        private void AcceptCallback(IAsyncResult asyncResult)
        {
            try
            {
                BufferTCP = new byte[4096];

                TcpClient tcpClient = ListnerTCP.EndAcceptTcpClient(asyncResult);

                tcpClient.ReceiveBufferSize = 4096;
                tcpClient.SendBufferSize = 4096;

                Console.WriteLine($"Peer connected with Endpoint: {tcpClient.Client.RemoteEndPoint.ToString()}");

                PeerConnected.Invoke(tcpClient);
                tcpClients.Add(tcpClient);

                tcpClient.Client.BeginReceive(BufferTCP, 0, BufferTCP.Length, SocketFlags.None, ReceiveCallback, tcpClient);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        private void ReceiveCallback(IAsyncResult asyncResult)
        {

            using (TcpClient? tcpClient = asyncResult.AsyncState as TcpClient)
            {
                PacketTCP = new Packet();

                try
                {
                    if (tcpClient == null || tcpClient.Client == null)
                    {
                        // handle null TcpClient object
                        return;
                    }

                    int receivedByteLength = tcpClient.Client.EndReceive(asyncResult);
                    if (receivedByteLength <= 0)
                    {
                        tcpClient.Close();
                        tcpClients.Remove(tcpClient);
                        PeerDisconnected.Invoke(tcpClient);

                        return;
                    }

                    byte[] receivedData = new byte[receivedByteLength];
                    Array.Copy(BufferTCP, receivedData, receivedByteLength);


                    PacketTCP.Reset(HandleReceviedData(receivedData));

                    tcpClient.Client.BeginReceive(BufferTCP, 0, BufferTCP.Length, SocketFlags.None, ReceiveCallback, tcpClient);
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error receiving TCP data: {_ex}");
                    Console.WriteLine($"Peer disconnected: {tcpClient?.Client?.RemoteEndPoint?.ToString()}");

                    tcpClient.Close();
                    tcpClients.Remove(tcpClient);
                    PeerDisconnected.Invoke(tcpClient);

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
                    Console.WriteLine($"PeerListnener - Packet received: {_packet.ToString()}");
                    PeerListenerPacketReceived?.Invoke(_packet);
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
