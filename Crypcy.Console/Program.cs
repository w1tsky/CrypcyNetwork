using Crypcy.Network.PeerItems;
using Crypcy.Network.PeerNetwork;
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Command list");
Console.WriteLine("1: Start Peer");
Console.WriteLine("2: Connect to Peer");
Console.WriteLine("3: List Connected Peers");
Console.WriteLine("4: Send message to Peer");
Console.WriteLine("5: Stop Peer");


OldPeer LocalPeer = null;


e: Console.WriteLine("Type 'exit' to shutdown the server");
switch (Console.ReadLine())
{
    case "EXIT":
        Console.WriteLine("Shutting down...");
        Environment.Exit(0);
        break;
    case "1":
        Console.WriteLine("Choose Server Port");

        int localPort = int.Parse(Console.ReadLine());

        LocalPeer = new OldPeer(new IPEndPoint(IPAddress.Any, localPort));
        LocalPeer.StartPeer();
        LocalPeer.PeerPacketReceived += DisplayRecievedMessage;

        goto e;
    case "2":
        Console.WriteLine("Select servers IP to connect");
        string ip = Console.ReadLine();

        Console.WriteLine("Select servers Port to connect");
        int peerToConnectPort = int.Parse(Console.ReadLine());

        IPEndPoint peerToConnectEndpoint = new IPEndPoint(IPAddress.Parse(ip), peerToConnectPort);

        LocalPeer.ConnectToPeer(peerToConnectEndpoint);

        goto e;

    case "3":
        Console.WriteLine("Connected Peer List:");


        foreach (var peer in LocalPeer.PeersConnected)
        {
            Console.WriteLine($"Peer:   {peer.Client.RemoteEndPoint} - connected: {peer.Client.Connected} ");
        }


        goto e;
    case "4":
        Console.WriteLine("Peers availible to connect:");

        Dictionary<int, TcpClient> peersConnectedList = LocalPeer.PeersConnected
                                         .Select((client, index) => new { index, client })
                                         .ToDictionary(i => i.index + 1, c => c.client);

        foreach (var peer in peersConnectedList)
        {
            Console.WriteLine($"Peer: {peer.Key} - {peer.Value.Client.RemoteEndPoint} - connected: {peer.Value.Client.Connected} ");
        }

        Console.WriteLine("Select peer to send message");
        string selectedPeer = Console.ReadLine();

        try
        {
            IPEndPoint remoteEndpoint = (IPEndPoint)peersConnectedList[Int32.Parse(selectedPeer)].Client.RemoteEndPoint;
            Console.WriteLine("Type message");

            string message = Console.ReadLine();

            Packet packet = new Packet();
            packet.Write(message);

            LocalPeer.SendPacketToPeer(remoteEndpoint, packet);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        goto e;
    case "5":
        Console.WriteLine("Stopping peer....");
        if (LocalPeer != null)
        {
            LocalPeer.StopPeer();
        }
        goto e;
    default:
        goto e;
}


void DisplayRecievedMessage(Packet packet)
{

    string message = Encoding.ASCII.GetString(packet.ToArray());
    Console.WriteLine(message);
}

