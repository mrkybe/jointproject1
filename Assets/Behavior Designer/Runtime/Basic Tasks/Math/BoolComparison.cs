using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Math
{
    [TaskCategory("Basic/Math")]
    [TaskDescription("Performs a comparison between two bools.")]
    public class BoolComparison : Conditional
    {
        [Tooltip("The first bool")]
        public SharedBool bool1;
        [Tooltip("The second bool")]
        public SharedBool bool2;

        public override TaskStatus OnUpdate()
        {
            return bool1.Value == bool2.Value ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            bool1.Value = false;
            bool2.Value = false;
        }
    }
}