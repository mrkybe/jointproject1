using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Transform
{
    [TaskCategory("Basic/Transform")]
    [TaskDescription("Applies a rotation. Returns Success.")]
    public class Rotate : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Amount to rotate")]
        public SharedVector3 eulerAngles;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Specifies which axis the rotation is relative to")]
        public Space relativeTo = Space.Self;

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

            targetTransform.Rotate(eulerAngles.Value, relativeTo);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            eulerAngles = UnityEngine.Vector3.zero;
            relativeTo = Space.Self;
        }
    }
}