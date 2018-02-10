using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Quaternion
{
    [TaskCategory("Basic/Quaternion")]
    [TaskDescription("Lerps between two quaternions.")]
    public class Lerp : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The from rotation")]
        public SharedQuaternion fromQuaternion;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The to rotation")]
        public SharedQuaternion toQuaternion;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The amount to lerp")]
        public SharedFloat amount;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored result")]
        [RequiredField]
        public SharedQuaternion storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Quaternion.Lerp(fromQuaternion.Value, toQuaternion.Value, amount.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            fromQuaternion = toQuaternion = storeResult = UnityEngine.Quaternion.identity;
            amount = 0;
        }
    }
}