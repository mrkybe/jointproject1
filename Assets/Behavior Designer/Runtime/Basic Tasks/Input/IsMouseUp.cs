using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Input
{
    [TaskCategory("Basic/Input")]
    [TaskDescription("Returns success when the specified mouse button is pressed.")]
    public class IsMouseUp : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The button index")]
        public SharedInt buttonIndex;

        public override TaskStatus OnUpdate()
        {
            return UnityEngine.Input.GetMouseButtonUp(buttonIndex.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            buttonIndex = 0;
        }
    }
}