using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.CharacterController
{
    [TaskCategory("Basic/CharacterController")]
    [TaskDescription("Returns Success if the collider hit another object, otherwise Failure.")]
    public class HasColliderHit : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The tag of the GameObject to check for a collision against")]
        public SharedString tag = "";
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The object that started the collision")]
        public SharedGameObject collidedGameObject;

        private bool enteredCollision = false;

        public override TaskStatus OnUpdate()
        {
            return enteredCollision ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnEnd()
        {
            enteredCollision = false;
        }

        public override void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (string.IsNullOrEmpty(tag.Value) || tag.Value.Equals(hit.gameObject.tag)) {
                collidedGameObject.Value = hit.gameObject;
                enteredCollision = true;
            }
        }

        public override void OnReset()
        {
            targetGameObject = null;
            tag = "";
            collidedGameObject = null;
        }
    }
}