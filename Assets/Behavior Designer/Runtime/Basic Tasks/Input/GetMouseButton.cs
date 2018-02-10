using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Input
{
    [TaskCategory("Basic/Input")]
    [TaskDescription("Stores the state of the specified mouse button.")]
    public class GetMouseButton : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The index of the button")]
        public SharedInt buttonIndex;
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored result")]
        public SharedBool storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Input.GetMouseButton(buttonIndex.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            buttonIndex = 0;
            storeResult = false;
        }
    }
}