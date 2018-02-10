using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Time
{
    [TaskCategory("Basic/Time")]
    [TaskDescription("Returns the real time in seconds since the game started.")]
    public class GetRealtimeSinceStartup : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The variable to store the result")]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Time.realtimeSinceStartup;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storeResult.Value = 0;
        }
    }
}