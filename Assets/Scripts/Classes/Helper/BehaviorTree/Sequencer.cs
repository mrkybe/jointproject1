﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Classes.Helper.BehaviorTree
{
    class Sequencer : Node
    {
        public Sequencer(Node parent)
        {
            Parent = parent;
        }
    }
}
