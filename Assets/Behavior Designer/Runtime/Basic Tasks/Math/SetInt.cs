using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Math
{
    [TaskCategory("Basic/Math")]
    [TaskDescription("Sets an int value")]
    public class SetInt : Action
    {
        [Tooltip("The int value to set")]
        public SharedInt intValue;
        [Tooltip("The variable to store the result")]
        public SharedInt storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = intValue.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            intValue.Value = 0;
            storeResult.Value = 0;
        }
    }
}