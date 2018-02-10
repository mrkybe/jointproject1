using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector3
{
    [TaskCategory("Basic/Vector3")]
    [TaskDescription("Normalize the Vector3.")]
    public class Normalize : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector3 to normalize")]
        public SharedVector3 vector3Variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The normalized resut")]
        [RequiredField]
        public SharedVector3 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Vector3.Normalize(vector3Variable.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector3Variable = storeResult = UnityEngine.Vector3.zero;
        }
    }
}