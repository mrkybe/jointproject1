using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Math
{
    [TaskCategory("Basic/Math")]
    [TaskDescription("Flips the value of the bool.")]
    public class BoolFlip : Action
    {
        [Tooltip("The bool to flip the value of")]
        public SharedBool boolVariable;

        public override TaskStatus OnUpdate()
        {
            boolVariable.Value = !boolVariable.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            boolVariable.Value = false;
        }
    }
}