using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Math
{
    [TaskCategory("Basic/Math")]
    [TaskDescription("Sets a bool value")]
    public class SetBool : Action
    {
        [Tooltip("The bool value to set")]
        public SharedBool boolValue;
        [Tooltip("The variable to store the result")]
        public SharedBool storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = boolValue.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            boolValue.Value = false;
            storeResult.Value = false;
        }
    }
}