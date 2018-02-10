using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Normalize the Vector2.")]
    public class Normalize : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector2 to normalize")]
        public SharedVector2 vector2Variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The normalized resut")]
        [RequiredField]
        public SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = vector2Variable.Value.normalized;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector2Variable = storeResult = UnityEngine.Vector2.zero;
        }
    }
}