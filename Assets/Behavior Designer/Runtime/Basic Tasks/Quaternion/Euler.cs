using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Quaternion
{
    [TaskCategory("Basic/Quaternion")]
    [TaskDescription("Stores the quaternion of a euler vector.")]
    public class Euler : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The euler vector")]
        public SharedVector3 eulerVector;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored quaternion")]
        [RequiredField]
        public SharedQuaternion storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Quaternion.Euler(eulerVector.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            eulerVector = UnityEngine.Vector3.zero;
            storeResult = UnityEngine.Quaternion.identity;
        }
    }
}