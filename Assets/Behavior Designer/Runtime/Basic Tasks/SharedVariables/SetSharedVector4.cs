using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Sets the SharedVector4 variable to the specified object. Returns Success.")]
    public class SetSharedVector4 : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The value to set the SharedVector4 to")]
        public SharedVector4 targetValue;
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The SharedVector4 to set")]
        public SharedVector4 targetVariable;

        public override TaskStatus OnUpdate()
        {
            targetVariable.Value = targetValue.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetValue = Vector4.zero;
            targetVariable = Vector4.zero;
        }
    }
}