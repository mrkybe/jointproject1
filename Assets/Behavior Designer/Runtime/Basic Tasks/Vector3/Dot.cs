using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector3
{
    [TaskCategory("Basic/Vector3")]
    [TaskDescription("Stores the dot product of two Vector3 values.")]
    public class Dot : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The left hand side of the dot product")]
        public SharedVector3 leftHandSide;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The right hand side of the dot product")]
        public SharedVector3 rightHandSide;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The dot product result")]
        [RequiredField]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Vector3.Dot(leftHandSide.Value, rightHandSide.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            leftHandSide = rightHandSide = UnityEngine.Vector3.zero;
            storeResult = 0;
        }
    }
}