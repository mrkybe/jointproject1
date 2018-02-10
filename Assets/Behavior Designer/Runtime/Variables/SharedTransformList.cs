using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Variables
{
    [System.Serializable]
    public class SharedTransformList : SharedVariable<List<Transform>>
    {
        public static implicit operator SharedTransformList(List<Transform> value) { return new SharedTransformList { mValue = value }; }
    }
}