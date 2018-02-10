using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Sets the value of the Vector2.")]
    public class SetValue : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector2 to get the values of")]
        public SharedVector2 vector2Value;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector2 to set the values of")]
        public SharedVector2 vector2Variable;

        public override TaskStatus OnUpdate()
        {
            vector2Variable.Value = vector2Value.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector2Value = vector2Variable = UnityEngine.Vector2.zero;
        }
    }
}