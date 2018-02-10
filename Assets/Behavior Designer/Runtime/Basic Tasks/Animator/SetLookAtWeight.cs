using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Animator
{
    [TaskCategory("Basic/Animator")]
    [TaskDescription("Sets the look at weight. Returns success immediately after.")]
    public class SetLookAtWeight : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("(0-1) the global weight of the LookAt, multiplier for other parameters.")]
        public SharedFloat weight;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("(0-1) determines how much the body is involved in the LookAt.")]
        public float bodyWeight;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("(0-1) determines how much the head is involved in the LookAt.")]
        public float headWeight = 1;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("(0-1) determines how much the eyes are involved in the LookAt.")]
        public float eyesWeight;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("(0-1) 0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped " +
                 "(look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).")]
        public float clampWeight = 0.5f;

        private UnityEngine.Animator animator;
        private UnityEngine.GameObject prevGameObject;
        private bool weightSet;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                animator = currentGameObject.GetComponent<UnityEngine.Animator>();
                prevGameObject = currentGameObject;
            }
            weightSet = false;
        }

        public override TaskStatus OnUpdate()
        {
            if (animator == null) {
                UnityEngine.Debug.LogWarning("Animator is null");
                return TaskStatus.Failure;
            }

            return weightSet ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnAnimatorIK()
        {
            if (animator == null) {
                return;
            }
            animator.SetLookAtWeight(weight.Value, bodyWeight, headWeight, eyesWeight, clampWeight);
            weightSet = true;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            weight = 0;
            bodyWeight = 0;
            headWeight = 1;
            eyesWeight = 0;
            clampWeight = 0.5f;
        }
    }
}