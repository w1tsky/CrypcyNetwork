using Crypcy.Network.PeerItems;
using Crypcy.Network.PeerNetwork.Interfaces.PeerTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.PeerTcp
{
    public class PeerTcpReceiveHandler : IPeerTcpReceiveHandler
    {
        private Packet PacketTCP;
        private byte[] BufferTCP;

        public delegate void PacketHandler(Packet _packet);
        public event PacketHandler PeerListenerPacketReceived;

        public void ReceiveHandler(IAsyncResult asyncResult)
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

                        return;
                    }

                    byte[] receivedData = new byte[receivedByteLength];
                    Array.Copy(BufferTCP, receivedData, receivedByteLength);


                    PacketTCP.Reset(ReceivedDataHandler(receivedData));

                    tcpClient.Client.BeginReceive(BufferTCP, 0, BufferTCP.Length, SocketFlags.None, ReceiveHandler, tcpClient);
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error receiving TCP data: {_ex}");
                    Console.WriteLine($"Peer disconnected: {tcpClient?.Client?.RemoteEndPoint?.ToString()}");

                    tcpClient.Close();

                }
            }
        }

        public bool ReceivedDataHandler(byte[] receivedData)
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
