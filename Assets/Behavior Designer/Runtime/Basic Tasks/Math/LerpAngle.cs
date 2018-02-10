using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Math
{
    [TaskCategory("Basic/Math")]
    [TaskDescription("Lerp the angle by an amount.")]
    public class LerpAngle : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The from value")]
        public SharedFloat fromValue;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The to value")]
        public SharedFloat toValue;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The amount to lerp")]
        public SharedFloat lerpAmount;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The lerp resut")]
        [RequiredField]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Mathf.LerpAngle(fromValue.Value, toValue.Value, lerpAmount.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            fromValue = toValue = lerpAmount = storeResult = 0;
        }
    }
}