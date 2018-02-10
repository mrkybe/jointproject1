using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Debug
{
    [TaskCategory("Basic/Debug")]
    [TaskDescription("Draws a debug ray")]
    public class DrawRay : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The position")]
        public SharedVector3 start;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The direction")]
        public SharedVector3 direction;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The color")]
        public SharedColor color = Color.white;

        public override TaskStatus OnUpdate()
        {
            UnityEngine.Debug.DrawRay(start.Value, direction.Value, color.Value);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            start = UnityEngine.Vector3.zero;
            direction = UnityEngine.Vector3.zero;
            color = Color.white;
        }
    }
}