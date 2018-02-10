using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Move from the current position to the target position.")]
    public class MoveTowards : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The current position")]
        public SharedVector2 currentPosition;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The target position")]
        public SharedVector2 targetPosition;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The movement speed")]
        public SharedFloat speed;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The move resut")]
        [RequiredField]
        public SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Vector2.MoveTowards(currentPosition.Value, targetPosition.Value, speed.Value * UnityEngine.Time.deltaTime);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            currentPosition = targetPosition = storeResult = UnityEngine.Vector2.zero;
            speed = 0;
        }
    }
}