using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Math
{
    [TaskCategory("Basic/Math")]
    [TaskDescription("Stores the absolute value of the float.")]
    public class FloatAbs : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The float to return the absolute value of")]
        public SharedFloat floatVariable;

        public override TaskStatus OnUpdate()
        {
            floatVariable.Value = Mathf.Abs(floatVariable.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            floatVariable = 0;
        }
    }
}