using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.String
{
    [TaskCategory("Basic/String")]
    [TaskDescription("Sets the variable string to the value string.")]
    public class SetString : Action
    {
        [Tooltip("The target string")]
        [RequiredField]
        public SharedString variable;
        [Tooltip("The value string")]
        public SharedString value;

        public override TaskStatus OnUpdate()
        {
            variable.Value = value.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            variable = "";
            value = "";
        }
    }
}