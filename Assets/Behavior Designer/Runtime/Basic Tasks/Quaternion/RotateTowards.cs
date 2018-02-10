using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Quaternion
{
    [TaskCategory("Basic/Quaternion")]
    [TaskDescription("Stores the quaternion after a rotation.")]
    public class RotateTowards : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The from rotation")]
        public SharedQuaternion fromQuaternion;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The to rotation")]
        public SharedQuaternion toQuaternion;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The maximum degrees delta")]
        public SharedFloat maxDeltaDegrees;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored result")]
        [RequiredField]
        public SharedQuaternion storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Quaternion.RotateTowards(fromQuaternion.Value, toQuaternion.Value, maxDeltaDegrees.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            fromQuaternion = toQuaternion = storeResult = UnityEngine.Quaternion.identity;
            maxDeltaDegrees = 0;
        }
    }
}