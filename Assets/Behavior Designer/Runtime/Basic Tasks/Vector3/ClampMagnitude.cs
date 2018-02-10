using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector3
{
    [TaskCategory("Basic/Vector3")]
    [TaskDescription("Clamps the magnitude of the Vector3.")]
    public class ClampMagnitude : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector3 to clamp the magnitude of")]
        public SharedVector3 vector3Variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The max length of the magnitude")]
        public SharedFloat maxLength;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The clamp magnitude resut")]
        [RequiredField]
        public SharedVector3 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Vector3.ClampMagnitude(vector3Variable.Value, maxLength.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector3Variable = storeResult = UnityEngine.Vector3.zero;
            maxLength = 0;
        }
    }
}