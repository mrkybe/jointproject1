using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Sets the SharedColor variable to the specified object. Returns Success.")]
    public class SetSharedColor : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The value to set the SharedColor to")]
        public SharedColor targetValue;
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The SharedColor to set")]
        public SharedColor targetVariable;

        public override TaskStatus OnUpdate()
        {
            targetVariable.Value = targetValue.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetValue = Color.black;
            targetVariable = Color.black;
        }
    }
}