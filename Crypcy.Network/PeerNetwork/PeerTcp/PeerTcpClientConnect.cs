using Crypcy.Network.PeerNetwork.Interfaces.PeerTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Crypcy.Network.PeerNetwork.PeerTcp
{
    public class PeerTcpClientConnect : IPeerTcpClientConnect
    {

        public TcpClient ClientTCP;

        public EndPoint LocalEndPoint;

        public delegate void PeerHandler(TcpClient tcpClient);

        public event PeerHandler ConnectedToPeer { add { } remove { } }

        public PeerTcpClientConnect(TcpClient tcpClient, EndPoint localEndPoint)
        {
            ClientTCP = tcpClient;
            LocalEndPoint = localEndPoint;
        }


        public void TcpConnect(IPEndPoint connectEndpoint)
        {
            try
            {
               
                //Init Peer Tcp Client
                ClientTCP = new TcpClient();

                ClientTCP.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                ClientTCP.Client.ExclusiveAddressUse = false;
                ClientTCP.Client.Bind(LocalEndPoint);

                ClientTCP.SendBufferSize = 4096;
                ClientTCP.ReceiveBufferSize = 4096;


                ClientTCP.BeginConnect(connectEndpoint.Address, connectEndpoint.Port, TcpConnectHandle, ClientTCP);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void TcpConnectHandle(IAsyncResult asyncResult)
        {
            TcpClient? tcpClient = asyncResult.AsyncState as TcpClient;

            try
            {
                tcpClient?.EndConnect(asyncResult);
                //ConnectedToPeer.Invoke(tcpClient);

                Console.WriteLine($"Succecefully conneted to peer {tcpClient.Client.RemoteEndPoint}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
