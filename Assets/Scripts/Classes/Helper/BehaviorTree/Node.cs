using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Classes.Helper.BehaviorTree
{
    abstract class Node
    {
        public string Name = "";
        public List<Node> Children = new List<Node>();
        public Node Parent = null;
        public Node Root = null;

        public Node()
        {
            throw new NotImplementedException();
        }

        void AddChild(Node child)
        {
            Children.Add(child);
        }
    }
}
