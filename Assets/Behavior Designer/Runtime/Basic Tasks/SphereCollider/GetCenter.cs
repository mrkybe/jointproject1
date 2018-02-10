using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.SphereCollider
{
    [TaskCategory("Basic/SphereCollider")]
    [TaskDescription("Stores the center of the SphereCollider. Returns Success.")]
    public class GetCenter : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The center of the SphereCollider")]
        [RequiredField]
        public SharedVector3 storeValue;

        private UnityEngine.SphereCollider sphereCollider;
        private UnityEngine.GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                sphereCollider = currentGameObject.GetComponent<UnityEngine.SphereCollider>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (sphereCollider == null) {
                UnityEngine.Debug.LogWarning("SphereCollider is null");
                return TaskStatus.Failure;
            }

            storeValue.Value = sphereCollider.center;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storeValue = UnityEngine.Vector3.zero;
        }
    }
}