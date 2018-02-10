using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Time
{
    [TaskCategory("Basic/Time")]
    [TaskDescription("Returns the time in second since the start of the game.")]
    public class GetTime : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The variable to store the result")]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Time.time;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storeResult.Value = 0;
        }
    }
}