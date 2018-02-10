using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Quaternion
{
    [TaskCategory("Basic/Quaternion")]
    [TaskDescription("Stores a rotation which rotates from the first direction to the second.")]
    public class FromToRotation : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The from rotation")]
        public SharedVector3 fromDirection;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The to rotation")]
        public SharedVector3 toDirection;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored result")]
        [RequiredField]
        public SharedQuaternion storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Quaternion.FromToRotation(fromDirection.Value, toDirection.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            fromDirection = toDirection = UnityEngine.Vector3.zero;
            storeResult = UnityEngine.Quaternion.identity;
        }
    }
}