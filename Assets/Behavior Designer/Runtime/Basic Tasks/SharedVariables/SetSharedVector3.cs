using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Sets the SharedVector3 variable to the specified object. Returns Success.")]
    public class SetSharedVector3 : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The value to set the SharedVector3 to")]
        public SharedVector3 targetValue;
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The SharedVector3 to set")]
        public SharedVector3 targetVariable;

        public override TaskStatus OnUpdate()
        {
            targetVariable.Value = targetValue.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetValue = UnityEngine.Vector3.zero;
            targetVariable = UnityEngine.Vector3.zero;
        }
    }
}