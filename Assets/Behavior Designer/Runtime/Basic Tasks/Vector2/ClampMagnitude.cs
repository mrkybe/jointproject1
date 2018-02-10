using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Clamps the magnitude of the Vector2.")]
    public class ClampMagnitude : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector2 to clamp the magnitude of")]
        public SharedVector2 vector2Variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The max length of the magnitude")]
        public SharedFloat maxLength;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The clamp magnitude resut")]
        [RequiredField]
        public SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Vector2.ClampMagnitude(vector2Variable.Value, maxLength.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector2Variable = storeResult = UnityEngine.Vector2.zero;
            maxLength = 0;
        }
    }
}