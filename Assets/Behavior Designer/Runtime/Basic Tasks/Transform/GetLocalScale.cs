using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Transform
{
    [TaskCategory("Basic/Transform")]
    [TaskDescription("Stores the local scale of the Transform. Returns Success.")]
    public class GetLocalScale : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The local scale of the Transform")]
        [RequiredField]
        public SharedVector3 storeValue;

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

            storeValue.Value = targetTransform.localScale;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            storeValue = UnityEngine.Vector3.zero;
        }
    }
}