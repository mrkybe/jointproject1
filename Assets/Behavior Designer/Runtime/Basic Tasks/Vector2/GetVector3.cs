using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Stores the Vector3 value of the Vector2.")]
    public class GetVector3 : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector2 to get the Vector3 value of")]
        public SharedVector2 vector3Variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector3 value")]
        [RequiredField]
        public SharedVector3 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = vector3Variable.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector3Variable = UnityEngine.Vector2.zero;
            storeResult = UnityEngine.Vector3.zero;
        }
    }
}