using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.GameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Returns Success if tags match, otherwise Failure.")]
    public class CompareTag : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The tag to compare against")]
        public SharedString tag;

        public override TaskStatus OnUpdate()
        {
            return GetDefaultGameObject(targetGameObject.Value).CompareTag(tag.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            tag = "";
        }
    }
}