using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Quaternion
{
    [TaskCategory("Basic/Quaternion")]
    [TaskDescription("Stores the angle in degrees between two rotations.")]
    public class Angle : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The first rotation")]
        public SharedQuaternion firstRotation;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The second rotation")]
        public SharedQuaternion secondRotation;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored result")]
        [RequiredField]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Quaternion.Angle(firstRotation.Value, secondRotation.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            firstRotation = secondRotation = UnityEngine.Quaternion.identity;
            storeResult = 0;
        }
    }
}