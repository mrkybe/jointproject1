using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.BoxCollider
{
    [TaskCategory("Basic/BoxCollider")]
    [TaskDescription("Sets the center of the BoxCollider. Returns Success.")]
    public class SetCenter : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The center of the BoxCollider")]
        public SharedVector3 center;

        private UnityEngine.BoxCollider boxCollider;
        private UnityEngine.GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                boxCollider = currentGameObject.GetComponent<UnityEngine.BoxCollider>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (boxCollider == null) {
                UnityEngine.Debug.LogWarning("BoxCollider is null");
                return TaskStatus.Failure;
            }

            boxCollider.center = center.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            center = UnityEngine.Vector3.zero;
        }
    }
}