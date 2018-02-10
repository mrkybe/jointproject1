using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Stores the square magnitude of the Vector2.")]
    public class GetSqrMagnitude : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector2 to get the square magnitude of")]
        public SharedVector2 vector2Variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The square magnitude of the vector")]
        [RequiredField]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = vector2Variable.Value.sqrMagnitude;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector2Variable = UnityEngine.Vector2.zero;
            storeResult = 0;
        }
    }
}