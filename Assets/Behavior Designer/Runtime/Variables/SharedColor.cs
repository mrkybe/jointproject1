using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Variables
{
    [System.Serializable]
    public class SharedColor : SharedVariable<Color>
    {
        public static implicit operator SharedColor(Color value) { return new SharedColor { mValue = value }; }
    }
}