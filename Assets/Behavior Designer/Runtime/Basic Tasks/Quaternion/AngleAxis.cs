using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Quaternion
{
    [TaskCategory("Basic/Quaternion")]
    [TaskDescription("Stores the rotation which rotates the specified degrees around the specified axis.")]
    public class AngleAxis : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The number of degrees")]
        public SharedFloat degrees;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The axis direction")]
        public SharedVector3 axis;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored result")]
        [RequiredField]
        public SharedQuaternion storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Quaternion.AngleAxis(degrees.Value, axis.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            degrees = 0;
            axis = UnityEngine.Vector3.zero;
            storeResult = UnityEngine.Quaternion.identity;
        }
    }
}