using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Input
{
    [TaskCategory("Basic/Input")]
    [TaskDescription("Stores the acceleration value.")]
    public class GetAcceleration : Action
    {
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored result")]
        public SharedVector3 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Input.acceleration;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storeResult = UnityEngine.Vector3.zero;
        }
    }
}