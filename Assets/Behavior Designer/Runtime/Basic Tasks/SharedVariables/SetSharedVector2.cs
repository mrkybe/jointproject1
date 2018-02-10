using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Sets the SharedVector2 variable to the specified object. Returns Success.")]
    public class SetSharedVector2 : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The value to set the SharedVector2 to")]
        public SharedVector2 targetValue;
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The SharedVector2 to set")]
        public SharedVector2 targetVariable;

        public override TaskStatus OnUpdate()
        {
            targetVariable.Value = targetValue.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetValue = UnityEngine.Vector2.zero;
            targetVariable = UnityEngine.Vector2.zero;
        }
    }
}