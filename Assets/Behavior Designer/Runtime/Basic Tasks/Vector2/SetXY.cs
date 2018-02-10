using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Sets the X and Y values of the Vector2.")]
    public class SetXY : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector2 to set the values of")]
        public SharedVector2 vector2Variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The X value. Set to None to have the value ignored")]
        public SharedFloat xValue;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Y value. Set to None to have the value ignored")]
        public SharedFloat yValue;

        public override TaskStatus OnUpdate()
        {
            var vector2Value = vector2Variable.Value;
            if (!xValue.IsNone) {
                vector2Value.x = xValue.Value;
            }
            if (!yValue.IsNone) {
                vector2Value.y = yValue.Value;
            }
            vector2Variable.Value = vector2Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector2Variable = UnityEngine.Vector2.zero;
            xValue = yValue = 0;
        }
    }
}