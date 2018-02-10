using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector3
{
    [TaskCategory("Basic/Vector3")]
    [TaskDescription("Multiply the Vector3 by a float.")]
    public class Multiply : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector3 to multiply of")]
        public SharedVector3 vector3Variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The value to multiply the Vector3 of")]
        public SharedFloat multiplyBy;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The multiplication resut")]
        [RequiredField]
        public SharedVector3 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = vector3Variable.Value * multiplyBy.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector3Variable = storeResult = UnityEngine.Vector3.zero;
            multiplyBy = 0;
        }
    }
}