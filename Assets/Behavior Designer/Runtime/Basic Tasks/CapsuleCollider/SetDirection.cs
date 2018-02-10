using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.CapsuleCollider
{
    [TaskCategory("Basic/CapsuleCollider")]
    [TaskDescription("Sets the direction of the CapsuleCollider. Returns Success.")]
    public class SetDirection : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The direction of the CapsuleCollider")]
        public SharedInt direction;

        private UnityEngine.CapsuleCollider capsuleCollider;
        private UnityEngine.GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                capsuleCollider = currentGameObject.GetComponent<UnityEngine.CapsuleCollider>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (capsuleCollider == null) {
                UnityEngine.Debug.LogWarning("CapsuleCollider is null");
                return TaskStatus.Failure;
            }

            capsuleCollider.direction = direction.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            direction = 0;
        }
    }
}