using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.GameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Returns the component of Type type if the game object has one attached, null if it doesn't. Returns Success.")]
    public class GetComponent : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The type of component")]
        public SharedString type;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The component")]
        [RequiredField]
        public SharedObject storeValue;

        public override TaskStatus OnUpdate()
        {
            storeValue.Value = GetDefaultGameObject(targetGameObject.Value).GetComponent(type.Value);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            type.Value = "";
            storeValue.Value = null;
        }
    }
}