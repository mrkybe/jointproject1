using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Transform
{
    [TaskCategory("Basic/Transform")]
    [TaskDescription("Gets the Angle between a GameObject's forward direction and a target. Returns Success.")]
    public class GetAngleToTarget : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The target object to measure the angle to. If null the targetPosition will be used.")]
        public SharedGameObject targetObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The world position to measure an angle to. If the targetObject is also not null, this value is used as an offset from that object's position.")]
        public SharedVector3 targetPosition;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Ignore height differences when calculating the angle?")]
        public SharedBool ignoreHeight = true;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The angle to the target")]
        [RequiredField]
        public SharedFloat storeValue;

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

            UnityEngine.Vector3 targetPos;
            if (targetObject.Value != null) {
                targetPos = targetObject.Value.transform.InverseTransformPoint(targetPosition.Value);
            } else {
                targetPos = targetPosition.Value;
            }

            if (ignoreHeight.Value) {
                targetPos.y = targetTransform.position.y;
            }

            var targetDir = targetPos - targetTransform.position;
            storeValue.Value = UnityEngine.Vector3.Angle(targetDir, targetTransform.forward);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            targetObject = null;
            targetPosition = UnityEngine.Vector3.zero;
            ignoreHeight = true;
            storeValue = 0;
        }
    }
}