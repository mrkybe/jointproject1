using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Animator
{
    [TaskCategory("Basic/Animator")]
    [TaskDescription("Interrupts the automatic target matching. Returns Success.")]
    public class InterruptMatchTarget : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("CompleteMatch will make the gameobject match the target completely at the next frame")]
        public bool completeMatch = true;

        private UnityEngine.Animator animator;
        private UnityEngine.GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                animator = currentGameObject.GetComponent<UnityEngine.Animator>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (animator == null) {
                UnityEngine.Debug.LogWarning("Animator is null");
                return TaskStatus.Failure;
            }

            animator.InterruptMatchTarget(completeMatch);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            completeMatch = true;
        }
    }
}