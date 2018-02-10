using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.BoxCollider2D
{
    [TaskCategory("Basic/BoxCollider2D")]
    [TaskDescription("Sets the size of the BoxCollider2D. Returns Success.")]
    public class SetSize : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The size of the BoxCollider2D")]
        public SharedVector2 size;

        private UnityEngine.BoxCollider2D boxCollider2D;
        private UnityEngine.GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                boxCollider2D = currentGameObject.GetComponent<UnityEngine.BoxCollider2D>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (boxCollider2D == null) {
                UnityEngine.Debug.LogWarning("BoxCollider2D is null");
                return TaskStatus.Failure;
            }

            boxCollider2D.size = size.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            size = UnityEngine.Vector2.zero;
        }
    }
}
