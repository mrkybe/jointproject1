using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Time
{
    [TaskCategory("Basic/Time")]
    [TaskDescription("Returns the time in seconds it took to complete the last frame.")]
    public class GetDeltaTime : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The variable to store the result")]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Time.deltaTime;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storeResult.Value = 0;
        }
    }
}