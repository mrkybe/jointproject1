using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Time
{
    [TaskCategory("Basic/Time")]
    [TaskDescription("Sets the scale at which time is passing.")]
    public class SetTimeScale : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The timescale")]
        public SharedFloat timeScale;

        public override TaskStatus OnUpdate()
        {
            UnityEngine.Time.timeScale = timeScale.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            timeScale.Value = 0;
        }
    }
}