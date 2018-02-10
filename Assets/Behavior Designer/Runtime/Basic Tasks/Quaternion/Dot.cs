using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Quaternion
{
    [TaskCategory("Basic/Quaternion")]
    [TaskDescription("Stores the dot product between two rotations.")]
    public class Dot : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The first rotation")]
        public SharedQuaternion leftRotation;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The second rotation")]
        public SharedQuaternion rightRotation;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored result")]
        [RequiredField]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Quaternion.Dot(leftRotation.Value, rightRotation.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            leftRotation = rightRotation = UnityEngine.Quaternion.identity;
            storeResult = 0;
        }
    }
}