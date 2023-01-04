using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore.Contracts.Network
{
	public interface IReciveData
	{
		event Action<string, byte[]> OnNewMessageRecived;
	}
}
