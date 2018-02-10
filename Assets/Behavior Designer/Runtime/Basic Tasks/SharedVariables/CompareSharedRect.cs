using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
    public class CompareSharedRect : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The first variable to compare")]
        public SharedRect variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The variable to compare to")]
        public SharedRect compareTo;

        public override TaskStatus OnUpdate()
        {
            return variable.Value.Equals(compareTo.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            variable = new Rect();
            compareTo = new Rect();
        }
    }
}