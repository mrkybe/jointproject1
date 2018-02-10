using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.GameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Sets the GameObject tag. Returns Success.")]
    public class SetTag : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject tag")]
        public SharedString tag;

        public override TaskStatus OnUpdate()
        {
            GetDefaultGameObject(targetGameObject.Value).tag = tag.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            tag = "";
        }
    }
}