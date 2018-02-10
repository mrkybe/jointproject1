using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Variables
{
    [System.Serializable]
    public class SharedObject : SharedVariable<Object>
    {
        public static explicit operator SharedObject(Object value) { return new SharedObject { mValue = value }; }
    }
}