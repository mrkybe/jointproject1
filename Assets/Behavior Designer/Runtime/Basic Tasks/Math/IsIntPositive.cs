using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Math
{
    [TaskCategory("Basic/Math")]
    [TaskDescription("Is the int a positive value?")]
    public class IsIntPositive : Conditional
    {
        [Tooltip("The int to check if positive")]
        public SharedInt intVariable;

        public override TaskStatus OnUpdate()
        {
            return intVariable.Value > 0 ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            intVariable = 0;
        }
    }
}