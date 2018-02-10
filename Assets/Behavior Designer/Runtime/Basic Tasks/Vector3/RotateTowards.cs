using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector3
{
    [TaskCategory("Basic/Vector3")]
    [TaskDescription("Rotate the current rotation to the target rotation.")]
    public class RotateTowards : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The current rotation in euler angles")]
        public SharedVector3 currentRotation;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The target rotation in euler angles")]
        public SharedVector3 targetRotation;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The maximum delta of the degrees")]
        public SharedFloat maxDegreesDelta;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The maximum delta of the magnitude")]
        public SharedFloat maxMagnitudeDelta;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The rotation resut")]
        [RequiredField]
        public SharedVector3 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Vector3.RotateTowards(currentRotation.Value, targetRotation.Value, maxDegreesDelta.Value * Mathf.Deg2Rad * UnityEngine.Time.deltaTime, maxMagnitudeDelta.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            currentRotation = targetRotation = storeResult = UnityEngine.Vector3.zero;
            maxDegreesDelta = maxMagnitudeDelta = 0;
        }
    }
}