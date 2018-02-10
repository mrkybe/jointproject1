using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.CharacterController
{
    [TaskCategory("Basic/CharacterController")]
    [TaskDescription("Sets the height of the CharacterController. Returns Success.")]
    public class SetHeight : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The height of the CharacterController")]
        public SharedFloat height;

        private UnityEngine.CharacterController characterController;
        private UnityEngine.GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                characterController = currentGameObject.GetComponent<UnityEngine.CharacterController>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (characterController == null) {
                UnityEngine.Debug.LogWarning("CharacterController is null");
                return TaskStatus.Failure;
            }

            characterController.height = height.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            height = 0;
        }
    }
}