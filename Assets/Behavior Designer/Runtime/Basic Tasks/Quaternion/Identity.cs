using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Quaternion
{
    [TaskCategory("Basic/Quaternion")]
    [TaskDescription("Stores the quaternion identity.")]
    public class Identity : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The identity")]
        [RequiredField]
        public SharedQuaternion storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Quaternion.identity;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storeResult = UnityEngine.Quaternion.identity;
        }
    }
}