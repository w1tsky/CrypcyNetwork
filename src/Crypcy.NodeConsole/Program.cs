using Crypcy.ApplicationCore;
using Crypcy.Communication.Network;

namespace Crypcy.NodeConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Short manual:");
            Console.WriteLine("Type \"start:PORT\" to start node with port(1024 to 49151).");
            Console.WriteLine("Type \"l\" to show list of nodes.");
            Console.WriteLine("For sending message you must write message in specific format:");
            Console.WriteLine("Template: Nmsg:your message");
            Console.WriteLine("N is number of target node, (you can get nodes via l command) your message is your message");
            Console.WriteLine("Server starting...");

            // DI:
            var console = new ConsoleImp();
            var network = new TcpNetwork();
            var nodes = new Nodes(network, console);
            // end DI

            
            

            while (true)
            {
                console.NewInput(Console.ReadLine());
            }
        }
    }
}