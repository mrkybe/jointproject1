using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Transform
{
    [TaskCategory("Basic/Transform")]
    [TaskDescription("Rotates the transform so the forward vector points at worldPosition. Returns Success.")]
    public class LookAt : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject to look at. If null the world position will be used.")]
        public SharedGameObject targetLookAt;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Point to look at")]
        public SharedVector3 worldPosition;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Vector specifying the upward direction")]
        public UnityEngine.Vector3 worldUp;

        private UnityEngine.Transform targetTransform;
        private UnityEngine.GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                targetTransform = currentGameObject.GetComponent<UnityEngine.Transform>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (targetTransform == null) {
                UnityEngine.Debug.LogWarning("Transform is null");
                return TaskStatus.Failure;
            }

            if (targetLookAt.Value != null) {
                targetTransform.LookAt(worldPosition.Value, worldUp);
            } else {
                targetTransform.LookAt(targetLookAt.Value.transform);
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            targetLookAt = null;
            worldPosition = UnityEngine.Vector3.up;
            worldUp = UnityEngine.Vector3.up;
        }
    }
}