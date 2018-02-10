using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Rigidbody
{
    [TaskCategory("Basic/Rigidbody")]
    [TaskDescription("Stores the velocity of the Rigidbody. Returns Success.")]
    public class GetVelocity : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The velocity of the Rigidbody")]
        [RequiredField]
        public SharedVector3 storeValue;

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

            storeValue.Value = rigidbody.velocity;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            storeValue = UnityEngine.Vector3.zero;
        }
    }
}