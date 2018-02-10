﻿using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
#if !(UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4)

#endif

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.NavMeshAgent
{
    [TaskCategory("Basic/NavMeshAgent")]
    [TaskDescription("Clears the current path. Returns Success.")]
    public class ResetPath : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;

        // cache the navmeshagent component
        private UnityEngine.AI.NavMeshAgent navMeshAgent;
        private UnityEngine.GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                navMeshAgent = currentGameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (navMeshAgent == null) {
                UnityEngine.Debug.LogWarning("NavMeshAgent is null");
                return TaskStatus.Failure;
            }

            navMeshAgent.ResetPath();

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
        }
    }
}