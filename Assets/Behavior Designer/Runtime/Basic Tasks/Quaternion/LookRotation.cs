using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Quaternion
{
    [TaskCategory("Basic/Quaternion")]
    [TaskDescription("Stores the quaternion of a forward vector.")]
    public class LookRotation : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The forward vector")]
        public SharedVector3 forwardVector;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The second Vector3")]
        public SharedVector3 secondVector3;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored quaternion")]
        [RequiredField]
        public SharedQuaternion storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Quaternion.LookRotation(forwardVector.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            forwardVector = UnityEngine.Vector3.zero;
            storeResult = UnityEngine.Quaternion.identity;
        }
    }
}