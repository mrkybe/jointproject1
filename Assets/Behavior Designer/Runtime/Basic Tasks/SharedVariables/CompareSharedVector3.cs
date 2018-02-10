using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
    public class CompareSharedVector3 : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The first variable to compare")]
        public SharedVector3 variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The variable to compare to")]
        public SharedVector3 compareTo;

        public override TaskStatus OnUpdate()
        {
            return variable.Value.Equals(compareTo.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            variable = UnityEngine.Vector3.zero;
            compareTo = UnityEngine.Vector3.zero;
        }
    }
}