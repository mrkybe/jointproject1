using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.GameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Sends a message to the target GameObject. Returns Success.")]
    public class SendMessage : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The message to send")]
        public SharedString message;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The value to send")]
        public SharedGenericVariable value;

        public override TaskStatus OnUpdate()
        {
            if (value.Value != null) {
                GetDefaultGameObject(targetGameObject.Value).SendMessage(message.Value, value.Value.value.GetValue());
            } else {
                GetDefaultGameObject(targetGameObject.Value).SendMessage(message.Value);
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            message = "";
        }
    }
}