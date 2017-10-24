using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Classes.Helper.BehaviorTree
{
    abstract class Node
    {
        public enum AIState { READY, RUNNING, DONE }

        public string Name = "";
        public List<Node> Children = new List<Node>();
        public Node Parent = null;
        public Node Root = null;
        public AIState State = AIState.READY;
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
