using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.NodeUI.Models
{
    public class NodeGroup
    {
        public string Name { get; set; }
        public List<string> Nodes { get; set; } 
        public List<Message> Messages { get; set; }
    }
}
