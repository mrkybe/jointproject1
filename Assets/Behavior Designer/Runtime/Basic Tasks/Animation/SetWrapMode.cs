using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Animation
{
    [TaskCategory("Basic/Animation")]
    [TaskDescription("Sets the wrap mode to the specified value. Returns Success.")]
    public class SetWrapMode : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("How should time beyond the playback range of the clip be treated?")]
        public WrapMode wrapMode = WrapMode.Default;

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

            animation.wrapMode = wrapMode;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            wrapMode = WrapMode.Default;
        }
    }
}