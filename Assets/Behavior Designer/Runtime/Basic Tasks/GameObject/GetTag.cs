using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.GameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Stores the GameObject tag. Returns Success.")]
    public class GetTag : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Active state of the GameObject")]
        [RequiredField]
        public SharedString storeValue;

        public override TaskStatus OnUpdate()
        {
            storeValue.Value = GetDefaultGameObject(targetGameObject.Value).tag;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            storeValue = "";
        }
    }
}