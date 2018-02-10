using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector3
{
    [TaskCategory("Basic/Vector3")]
    [TaskDescription("Stores the Vector2 value of the Vector3.")]
    public class GetVector2 : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector3 to get the Vector2 value of")]
        public SharedVector3 vector3Variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector2 value")]
        [RequiredField]
        public SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = vector3Variable.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector3Variable = UnityEngine.Vector3.zero;
            storeResult = UnityEngine.Vector2.zero;
        }
    }
}