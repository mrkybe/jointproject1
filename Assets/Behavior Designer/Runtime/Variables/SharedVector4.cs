using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Variables
{
    [System.Serializable]
    public class SharedVector4 : SharedVariable<Vector4>
    {
        public static implicit operator SharedVector4(Vector4 value) { return new SharedVector4 { mValue = value }; }
    }
}