using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore.Contracts.Network
{
	public interface ISendData
	{
		void SendMessage(string node, byte[] message);
	}
}
