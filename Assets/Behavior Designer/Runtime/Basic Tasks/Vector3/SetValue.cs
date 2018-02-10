using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector3
{
    [TaskCategory("Basic/Vector3")]
    [TaskDescription("Sets the value of the Vector3.")]
    public class SetValue : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector3 to get the values of")]
        public SharedVector3 vector3Value;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector3 to set the values of")]
        public SharedVector3 vector3Variable;

        public override TaskStatus OnUpdate()
        {
            vector3Variable.Value = vector3Value.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector3Value = vector3Variable = UnityEngine.Vector3.zero;
        }
    }
}