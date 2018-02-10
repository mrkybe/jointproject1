using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.CharacterController
{
    [TaskCategory("Basic/CharacterController")]
    [TaskDescription("Moves the character with speed. Returns Success.")]
    public class SimpleMove : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The speed of the movement")]
        public SharedVector3 speed;

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

            characterController.SimpleMove(speed.Value);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            speed = UnityEngine.Vector3.zero;
        }
    }
}