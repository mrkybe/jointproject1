using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BehaviorDesigner.Runtime;

namespace Assets.Behavior_Designer.Runtime.Variables
{
    [System.Serializable]
    class SharedStringList : SharedVariable<List<string>>
    {
        public static implicit operator SharedStringList(List<string> value)
        {
            return new SharedStringList { Value = value };
        }
    }
}
