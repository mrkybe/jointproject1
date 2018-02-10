using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.AudioSource
{
    [TaskCategory("Basic/AudioSource")]
    [TaskDescription("Sets the mute value of the AudioSource. Returns Success.")]
    public class SetMute : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The mute value of the AudioSource")]
        public SharedBool mute;

        private UnityEngine.AudioSource audioSource;
        private UnityEngine.GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                audioSource = currentGameObject.GetComponent<UnityEngine.AudioSource>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (audioSource == null) {
                UnityEngine.Debug.LogWarning("AudioSource is null");
                return TaskStatus.Failure;
            }

            audioSource.mute = mute.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            mute = false;
        }
    }
}