using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Quaternion
{
    [TaskCategory("Basic/Quaternion")]
    [TaskDescription("Stores the inverse of the specified quaternion.")]
    public class Inverse : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The target quaternion")]
        public SharedQuaternion targetQuaternion;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored quaternion")]
        [RequiredField]
        public SharedQuaternion storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Quaternion.Inverse(targetQuaternion.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetQuaternion = storeResult = UnityEngine.Quaternion.identity;
        }
    }
}