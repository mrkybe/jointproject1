using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Transform
{
    [TaskCategory("Basic/Transform")]
    [TaskDescription("Moves the transform in the direction and distance of translation. Returns Success.")]
    public class Translate : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Move direction and distance")]
        public SharedVector3 translation;
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

            targetTransform.Translate(translation.Value, relativeTo);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            translation = UnityEngine.Vector3.zero;
            relativeTo = Space.Self;
        }
    }
}