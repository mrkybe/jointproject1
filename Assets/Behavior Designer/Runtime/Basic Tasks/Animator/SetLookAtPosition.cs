using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Animator
{
    [TaskCategory("Basic/Animator")]
    [TaskDescription("Sets the look at position. Returns Success.")]
    public class SetLookAtPosition : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The position to lookAt")]
        public SharedVector3 position;

        private UnityEngine.Animator animator;
        private UnityEngine.GameObject prevGameObject;
        private bool positionSet;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                animator = currentGameObject.GetComponent<UnityEngine.Animator>();
                prevGameObject = currentGameObject;
            }
            positionSet = false;
        }

        public override TaskStatus OnUpdate()
        {
            if (animator == null) {
                UnityEngine.Debug.LogWarning("Animator is null");
                return TaskStatus.Failure;
            }

            return positionSet ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnAnimatorIK()
        {
            if (animator == null) {
                return;
            }
            animator.SetLookAtPosition(position.Value);
            positionSet = true;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            position = UnityEngine.Vector3.zero;
        }
    }
}