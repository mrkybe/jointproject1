using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Stores the right vector value.")]
    public class GetRightVector : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored result")]
        [RequiredField]
        public SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Vector2.right;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storeResult = UnityEngine.Vector2.zero;
        }
    }
}