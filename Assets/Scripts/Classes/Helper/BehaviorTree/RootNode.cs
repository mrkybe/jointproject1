using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Classes.Helper.BehaviorTree
{
    class RootNode : Node
    {
        HashSet<List<string>> stacks = new HashSet<List<string>>();

        public RootNode(string name)
        {
            Name = name;
        }
    }
}
