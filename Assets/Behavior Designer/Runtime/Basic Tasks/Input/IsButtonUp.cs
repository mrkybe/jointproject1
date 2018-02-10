using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Input
{
    [TaskCategory("Basic/Input")]
    [TaskDescription("Returns success when the specified button is released.")]
    public class IsButtonUp : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The name of the button")]
        public SharedString buttonName;

        public override TaskStatus OnUpdate()
        {
            return UnityEngine.Input.GetButtonUp(buttonName.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            buttonName = "Fire1";
        }
    }
}