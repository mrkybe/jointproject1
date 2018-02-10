using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.AudioSource
{
    [TaskCategory("Basic/AudioSource")]
    [TaskDescription("Sets the clip value of the AudioSource. Returns Success.")]
    public class SetAudioClip : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The AudioSource clip")]
        public AudioClip audioClip;

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

            audioSource.clip = audioClip;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            audioClip = null;
        }
    }
}