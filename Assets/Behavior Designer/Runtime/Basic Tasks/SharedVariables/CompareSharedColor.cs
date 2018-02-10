using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
    public class CompareSharedColor : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The first variable to compare")]
        public SharedColor variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The variable to compare to")]
        public SharedColor compareTo;

        public override TaskStatus OnUpdate()
        {
            return variable.Value.Equals(compareTo.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            variable = Color.black;
            compareTo = Color.black;
        }
    }
}