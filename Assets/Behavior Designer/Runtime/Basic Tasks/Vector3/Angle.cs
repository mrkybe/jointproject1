﻿using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector3
{
    [TaskCategory("Basic/Vector3")]
    [TaskDescription("Returns the angle between two Vector3s.")]
    public class Angle : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The first Vector3")]
        public SharedVector3 firstVector3;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The second Vector3")]
        public SharedVector3 secondVector3;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The angle")]
        [RequiredField]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Vector3.Angle(firstVector3.Value, secondVector3.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            firstVector3 = secondVector3 = UnityEngine.Vector3.zero;
            storeResult = 0;
        }
    }
}
