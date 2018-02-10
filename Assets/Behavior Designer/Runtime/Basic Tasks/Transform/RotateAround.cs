using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Transform
{
    [TaskCategory("Basic/Transform")]
    [TaskDescription("Applies a rotation. Returns Success.")]
    public class RotateAround : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Point to rotate around")]
        public SharedVector3 point;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Axis to rotate around")]
        public SharedVector3 axis;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Amount to rotate")]
        public SharedFloat angle;

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

            targetTransform.RotateAround(point.Value, axis.Value, angle.Value);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            point = UnityEngine.Vector3.zero;
            axis = UnityEngine.Vector3.zero;
            angle = 0;
        }
    }
}