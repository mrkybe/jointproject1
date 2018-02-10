using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Multiply the Vector2 by a float.")]
    public class Multiply : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector2 to multiply of")]
        public SharedVector2 vector2Variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The value to multiply the Vector2 of")]
        public SharedFloat multiplyBy;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The multiplication resut")]
        [RequiredField]
        public SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = vector2Variable.Value * multiplyBy.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector2Variable = storeResult = UnityEngine.Vector2.zero;
            multiplyBy = 0;
        }
    }
}