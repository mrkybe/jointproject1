using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Behavior_Designer.Runtime.Actions.Custom.Planet
{
    [TaskCategory("Custom/Planet")]
    [TaskDescription("Stores the position of the Planet. Returns Success.")]
    class GetPosition : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Planet that the task operates on. If null the task GameObject is used.")]
        public SharedPlanet TargetPlanet;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Can the target Planet be empty?")]
        [RequiredField]
        public SharedVector3 storeValue;

        private Transform targetTransform;
        private global::Planet prevPlanet;

        public override void OnStart()
        {
            var currentPlanet = TargetPlanet.Value;
            if (currentPlanet != prevPlanet)
            {
                targetTransform = currentPlanet.transform;
                prevPlanet = TargetPlanet.Value;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (targetTransform == null)
            {
                Debug.LogWarning("Planet is null");
                return TaskStatus.Failure;
            }

            storeValue.Value = targetTransform.position;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            TargetPlanet = null;
            storeValue = Vector3.zero;
        }
    }
}
