using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Rigidbody
{
    [TaskCategory("Basic/Rigidbody")]
    [TaskDescription("Applies a force to the rigidbody that simulates explosion effects. Returns Success.")]
    public class AddExplosionForce : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The force of the explosion")]
        public SharedFloat explosionForce;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The position of the explosion")]
        public SharedVector3 explosionPosition;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The radius of the explosion")]
        public SharedFloat explosionRadius;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Applies the force as if it was applied from beneath the object")]
        public float upwardsModifier = 0;
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

            rigidbody.AddExplosionForce(explosionForce.Value, explosionPosition.Value, explosionRadius.Value, upwardsModifier, forceMode);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            explosionForce = 0;
            explosionPosition = UnityEngine.Vector3.zero;
            explosionRadius = 0;
            upwardsModifier = 0;
            forceMode = ForceMode.Force;
        }
    }
}