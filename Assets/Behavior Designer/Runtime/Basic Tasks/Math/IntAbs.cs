using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Math
{
    [TaskCategory("Basic/Math")]
    [TaskDescription("Stores the absolute value of the int.")]
    public class IntAbs : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The int to return the absolute value of")]
        public SharedInt intVariable;

        public override TaskStatus OnUpdate()
        {
            intVariable.Value = Mathf.Abs(intVariable.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            intVariable = 0;
        }
    }
}