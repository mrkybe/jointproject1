using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.BoxCollider
{
    [TaskCategory("Basic/BoxCollider")]
    [TaskDescription("Stores the center of the BoxCollider. Returns Success.")]
    public class GetCenter : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The center of the BoxCollider")]
        [RequiredField]
        public SharedVector3 storeValue;

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

            storeValue.Value = boxCollider.center;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            storeValue = UnityEngine.Vector3.zero;
        }
    }
}