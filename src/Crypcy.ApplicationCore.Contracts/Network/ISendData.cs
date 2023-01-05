using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore.Contracts.Network
{
	public interface ISendData
	{
		ValueTask SendMessageAsync(string node, byte[] message);
	}
}
