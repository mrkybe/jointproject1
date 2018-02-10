using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Math
{
    [TaskCategory("Basic/Math")]
    [TaskDescription("Clamps the float between two values.")]
    public class FloatClamp : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The float to clamp")]
        public SharedFloat floatVariable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The maximum value of the float")]
        public SharedFloat minValue;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The maximum value of the float")]
        public SharedFloat maxValue;

        public override TaskStatus OnUpdate()
        {
            floatVariable.Value = Mathf.Clamp(floatVariable.Value, minValue.Value, maxValue.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            floatVariable = minValue = maxValue = 0;
        }
    }
}