using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Input
{
    [TaskCategory("Basic/Input")]
    [TaskDescription("Stores the mouse position.")]
    public class GetMousePosition : Action
    {
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored result")]
        public SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Input.mousePosition;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storeResult = UnityEngine.Vector2.zero;
        }
    }
}