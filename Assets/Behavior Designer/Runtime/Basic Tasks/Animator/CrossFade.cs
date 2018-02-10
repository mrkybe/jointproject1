using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Animator
{
    [TaskCategory("Basic/Animator")]
    [TaskDescription("Creates a dynamic transition between the current state and the destination state. Returns Success.")]
    public class CrossFade : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The name of the state")]
        public SharedString stateName;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The duration of the transition. Value is in source state normalized time")]
        public SharedFloat transitionDuration;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The layer where the state is")]
        public int layer = -1;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The normalized time at which the state will play")]
        public float normalizedTime = float.NegativeInfinity;

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

            animator.CrossFade(stateName.Value, transitionDuration.Value, layer, normalizedTime);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            stateName = "";
            transitionDuration = 0;
            layer = -1;
            normalizedTime = float.NegativeInfinity;
        }
    }
}
