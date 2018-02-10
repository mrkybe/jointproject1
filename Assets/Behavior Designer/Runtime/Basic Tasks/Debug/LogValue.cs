using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Debug
{
    [TaskCategory("Basic/Debug")]
    [TaskDescription("Log a variable value.")]
    public class LogValue : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The variable to output")]
        public SharedGenericVariable variable;

        public override TaskStatus OnUpdate()
        {
            UnityEngine.Debug.Log(variable.Value.value.GetValue());

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            variable = null;
        }
    }
}