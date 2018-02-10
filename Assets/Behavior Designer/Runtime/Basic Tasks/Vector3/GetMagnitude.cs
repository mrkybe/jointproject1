using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector3
{
    [TaskCategory("Basic/Vector3")]
    [TaskDescription("Stores the magnitude of the Vector3.")]
    public class GetMagnitude : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector3 to get the magnitude of")]
        public SharedVector3 vector3Variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The magnitude of the vector")]
        [RequiredField]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = vector3Variable.Value.magnitude;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector3Variable = UnityEngine.Vector3.zero;
            storeResult = 0;
        }
    }
}