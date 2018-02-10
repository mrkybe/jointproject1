using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Stores the dot product of two Vector2 values.")]
    public class Dot : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The left hand side of the dot product")]
        public SharedVector2 leftHandSide;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The right hand side of the dot product")]
        public SharedVector2 rightHandSide;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The dot product result")]
        [RequiredField]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Vector2.Dot(leftHandSide.Value, rightHandSide.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            leftHandSide = rightHandSide = UnityEngine.Vector2.zero;
            storeResult = 0;
        }
    }
}