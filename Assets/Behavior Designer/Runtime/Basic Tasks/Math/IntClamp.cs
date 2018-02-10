using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Math
{
    [TaskCategory("Basic/Math")]
    [TaskDescription("Clamps the int between two values.")]
    public class IntClamp : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The int to clamp")]
        public SharedInt intVariable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The maximum value of the int")]
        public SharedInt minValue;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The maximum value of the int")]
        public SharedInt maxValue;

        public override TaskStatus OnUpdate()
        {
            intVariable.Value = Mathf.Clamp(intVariable.Value, minValue.Value, maxValue.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            intVariable = minValue = maxValue = 0;
        }
    }
}