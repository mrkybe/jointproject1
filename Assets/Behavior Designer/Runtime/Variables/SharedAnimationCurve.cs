using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Variables
{
    [System.Serializable]
    public class SharedAnimationCurve : SharedVariable<AnimationCurve>
    {
        public static implicit operator SharedAnimationCurve(AnimationCurve value) { return new SharedAnimationCurve { mValue = value }; }
    }
}