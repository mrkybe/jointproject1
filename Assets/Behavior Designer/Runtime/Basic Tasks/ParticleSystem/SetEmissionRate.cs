using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.ParticleSystem
{
    [TaskCategory("Basic/ParticleSystem")]
    [TaskDescription("Sets the emission rate of the Particle System.")]
    public class SetEmissionRate : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The emission rate of the ParticleSystem")]
        public SharedFloat emissionRate;

        private UnityEngine.ParticleSystem particleSystem;
        private UnityEngine.GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                particleSystem = currentGameObject.GetComponent<UnityEngine.ParticleSystem>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (particleSystem == null) {
                UnityEngine.Debug.LogWarning("ParticleSystem is null");
                return TaskStatus.Failure;
            }

#if !(UNITY_5_1 || UNITY_5_2)
            UnityEngine.Debug.Log("Warning: SetEmissionRate is not used in Unity 5.3 or later.");
#else
            particleSystem.emissionRate = emissionRate.Value;
#endif

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            emissionRate = 0;
        }
    }
}