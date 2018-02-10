using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Sets the SharedRect variable to the specified object. Returns Success.")]
    public class SetSharedRect : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The value to set the SharedRect to")]
        public SharedRect targetValue;
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The SharedRect to set")]
        public SharedRect targetVariable;

        public override TaskStatus OnUpdate()
        {
            targetVariable.Value = targetValue.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetValue = new Rect();
            targetVariable = new Rect();
        }
    }
}