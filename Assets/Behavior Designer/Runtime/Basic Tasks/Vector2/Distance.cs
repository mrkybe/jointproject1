using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Returns the distance between two Vector2s.")]
    public class Distance : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The first Vector2")]
        public SharedVector2 firstVector2;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The second Vector2")]
        public SharedVector2 secondVector2;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The distance")]
        [RequiredField]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Vector2.Distance(firstVector2.Value, secondVector2.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            firstVector2 = secondVector2 = UnityEngine.Vector2.zero;
            storeResult = 0;
        }
    }
}