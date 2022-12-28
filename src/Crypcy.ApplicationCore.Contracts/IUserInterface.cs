using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore.Contracts
{
	public interface IUserInterface
	{
		event Action<string, string> OnSendMessageRequest;
		event Action<int> OnStartNode;
		event Action<string, int> OnConnectToNodeRequest;
		void ShowMessage(string node, string message);
		void NodeConnectedNotification(string node);
		void NodeDiconnectedNotification(string node);
	}
}
