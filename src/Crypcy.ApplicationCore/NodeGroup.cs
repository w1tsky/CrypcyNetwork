using Crypcy.ApplicationCore.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore
{
    public class NodeGroup
    {
        protected HashSet<string> _nodes;
        protected Dictionary<string, HashSet<string>> _groups;

        public NodeGroup(HashSet<string> nodes)
        {   
            _nodes = nodes;
            _groups = new Dictionary<string, HashSet<string>>();
        }

        public void AddGroup(string groupName)
        {
            _groups.Add(groupName, new HashSet<string>());
        }
        public void AddGroupWithNodes(string groupName, HashSet<string> groupNodes)
        {
            _groups.Add(groupName, groupNodes);
        }

        public HashSet<string> GetGroup(string groupName)
        {
            return _groups[groupName];
        }

        public void AddNodeToGroup(string groupName, string node)
        {
            if (!_groups.ContainsKey(groupName))
            {
                _groups[groupName] = new HashSet<string>();
            }

            _groups[groupName].Add(node);
        }

    }
}
