using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Input
{
    [TaskCategory("Basic/Input")]
    [TaskDescription("Stores the state of the specified button.")]
    public class GetButton : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The name of the button")]
        public SharedString buttonName;
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored result")]
        public SharedBool storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Input.GetButton(buttonName.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            buttonName = "Fire1";
            storeResult = false;
        }
    }
}