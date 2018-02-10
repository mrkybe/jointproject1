using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Variables
{
    [System.Serializable]
    public class SharedObjectList : SharedVariable<List<Object>>
    {
        public static implicit operator SharedObjectList(List<Object> value) { return new SharedObjectList { mValue = value }; }
    }
}