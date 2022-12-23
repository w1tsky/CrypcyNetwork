using Crypcy.Network.PeerItems;
using Crypcy.Network.PeerNetwork.Interfaces.PeerTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.PeerTcp
{
    public class PeerTcpReceiveHandler
    {
        private byte[] TcpBuffer;

        public delegate void TcpPacketHandler(Packet packet);
        public event TcpPacketHandler TcpPacketReceived;

        public delegate void TcpConnectionHandler(TcpClient tcpClient);
        public event TcpConnectionHandler TcpDisconnected;

        public event EventHandler<string> OnResultsUpdate;

        public PeerTcpReceiveHandler(byte[] tcpBuffer)
        {
            TcpBuffer = tcpBuffer;
        }

        public void TcpReceiveHandler(IAsyncResult asyncResult)
        {

            using (TcpClient? tcpClient = asyncResult.AsyncState as TcpClient)
            {
                Packet tcpPacket = new Packet();

                try
                {
                    int receivedByteLength = tcpClient.Client.EndReceive(asyncResult);
                    if (receivedByteLength <= 0)
                    {
                        OnResultsUpdate.Invoke(this, ($"Peer disconnected: {tcpClient?.Client?.RemoteEndPoint?.ToString()} - No bytes received"));
                        TcpDisconnected?.Invoke(tcpClient);
                        tcpClient.Close();

                        return;
                    }

                    byte[] receivedData = new byte[receivedByteLength];
                    Array.Copy(TcpBuffer, receivedData, receivedByteLength);
                    tcpPacket.Reset(ReceivedDataHandler(tcpPacket, receivedData));
                    tcpClient.Client.BeginReceive(TcpBuffer, 0, TcpBuffer.Length, SocketFlags.None, TcpReceiveHandler, tcpClient);
                }
                catch (Exception _ex)
                {
                    OnResultsUpdate?.Invoke(this, ($"Error receiving TCP data: {_ex.Message}"));
                    OnResultsUpdate?.Invoke(this, ($"Peer disconnected: {tcpClient?.Client?.RemoteEndPoint?.ToString()}"));

                    TcpDisconnected?.Invoke(tcpClient);

                    tcpClient?.Close();

                }
            }
        }

        public bool ReceivedDataHandler(Packet tcpPacket, byte[] receivedData)
        {
            int _packetLength = 0;

            tcpPacket.SetBytes(receivedData);

            if (tcpPacket.UnreadLength() >= 4)
            {
                _packetLength = tcpPacket.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= tcpPacket.UnreadLength())
            {
                byte[] _packetBytes = tcpPacket.ReadBytes(_packetLength);

                using (Packet _packet = new Packet(_packetBytes))
                {
                    OnResultsUpdate?.Invoke(this, ($"PeerListnener - Packet received: {_packet.ToString()}"));
                    TcpPacketReceived?.Invoke(_packet);
                }


                _packetLength = 0;

                if (tcpPacket.UnreadLength() >= 4)
                {
                    _packetLength = tcpPacket.ReadInt();
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
