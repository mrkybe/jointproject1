using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector3
{
    [TaskCategory("Basic/Vector3")]
    [TaskDescription("Stores the up vector value.")]
    public class GetUpVector : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored result")]
        [RequiredField]
        public SharedVector3 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Vector3.up;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storeResult = UnityEngine.Vector3.zero;
        }
    }
}