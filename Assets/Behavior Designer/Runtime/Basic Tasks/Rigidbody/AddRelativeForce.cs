using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Rigidbody
{
    [TaskCategory("Basic/Rigidbody")]
    [TaskDescription("Applies a force to the rigidbody relative to its coordinate system. Returns Success.")]
    public class AddRelativeForce : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The amount of force to apply")]
        public SharedVector3 force;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The type of force")]
        public ForceMode forceMode = ForceMode.Force;

        // cache the rigidbody component
        private UnityEngine.Rigidbody rigidbody;
        private UnityEngine.GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                rigidbody = currentGameObject.GetComponent<UnityEngine.Rigidbody>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (rigidbody == null) {
                UnityEngine.Debug.LogWarning("Rigidbody is null");
                return TaskStatus.Failure;
            }

            rigidbody.AddRelativeForce(force.Value, forceMode);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            force = UnityEngine.Vector3.zero;
            forceMode = ForceMode.Force;
        }
    }
}