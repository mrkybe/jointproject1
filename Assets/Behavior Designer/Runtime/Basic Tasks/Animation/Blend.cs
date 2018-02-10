using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Animation
{
    [TaskCategory("Basic/Animation")]
    [TaskDescription("Blends the animation. Returns Success.")]
    public class Blend : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The name of the animation")]
        public SharedString animationName;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The weight the animation should blend to")]
        public float targetWeight = 1;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The amount of time it takes to blend")]
        public float fadeLength = 0.3f;

        // cache the animation component
        private UnityEngine.Animation animation;
        private UnityEngine.GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                animation = currentGameObject.GetComponent<UnityEngine.Animation>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (animation == null) {
                UnityEngine.Debug.LogWarning("Animation is null");
                return TaskStatus.Failure;
            }

            animation.Blend(animationName.Value, targetWeight, fadeLength);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            animationName = "";
            targetWeight = 1;
            fadeLength = 0.3f;
        }
    }
}