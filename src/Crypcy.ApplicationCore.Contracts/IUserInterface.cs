using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore.Contracts
{
<<<<<<< HEAD
    public interface IUserInterface
    {
        public event Action<string, string> OnSendMessageRequest;

        public event Action<int> OnStartNode;

        void ShowMessage(string node, string message);

        void NodeConnectedNotification(string node);

        void NodeDiconnectedNotification(string node);
    }
=======
	public interface IUserInterface
	{
		event Action<string, string> OnSendMessageRequest;
		event Action<int> OnStartNode;
		event Action<string, int> OnConnectToNodeRequest;
		void ShowMessage(string node, string message);
		void NodeConnectedNotification(string node);
		void NodeDiconnectedNotification(string node);
	}
>>>>>>> fe1e7c1dd8dd374db39247b012a505b26f9a010d
}
