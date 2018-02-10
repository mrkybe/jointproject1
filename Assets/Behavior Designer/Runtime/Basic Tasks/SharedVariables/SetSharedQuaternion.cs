using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Sets the SharedQuaternion variable to the specified object. Returns Success.")]
    public class SetSharedQuaternion : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The value to set the SharedQuaternion to")]
        public SharedQuaternion targetValue;
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The SharedQuaternion to set")]
        public SharedQuaternion targetVariable;

        public override TaskStatus OnUpdate()
        {
            targetVariable.Value = targetValue.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetValue = UnityEngine.Quaternion.identity;
            targetVariable = UnityEngine.Quaternion.identity;
        }
    }
}