using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Debug
{
    [TaskCategory("Basic/Debug")]
    [TaskDescription("Draws a debug line")]
    public class DrawLine : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The start position")]
        public SharedVector3 start;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The end position")]
        public SharedVector3 end;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The color")]
        public SharedColor color = Color.white;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Duration the line will be visible for in seconds.\nDefault: 0 means 1 frame.")]
        public SharedFloat duration;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Whether the line should show through world geometry.")]
        public SharedBool depthTest = true;

        public override TaskStatus OnUpdate()
        {
            UnityEngine.Debug.DrawLine(start.Value, end.Value, color.Value, duration.Value, depthTest.Value);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            start = UnityEngine.Vector3.zero;
            end = UnityEngine.Vector3.zero;
            color = Color.white;
            duration = 0f;
            depthTest = true;
        }
    }
}