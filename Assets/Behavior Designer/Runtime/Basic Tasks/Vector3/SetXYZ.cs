using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector3
{
    [TaskCategory("Basic/Vector3")]
    [TaskDescription("Sets the X, Y, and Z values of the Vector3.")]
    public class SetXYZ : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector3 to set the values of")]
        public SharedVector3 vector3Variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The X value. Set to None to have the value ignored")]
        public SharedFloat xValue;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Y value. Set to None to have the value ignored")]
        public SharedFloat yValue;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Z value. Set to None to have the value ignored")]
        public SharedFloat zValue;

        public override TaskStatus OnUpdate()
        {
            var vector3Value = vector3Variable.Value;
            if (!xValue.IsNone) {
                vector3Value.x = xValue.Value;
            }
            if (!yValue.IsNone) {
                vector3Value.y = yValue.Value;
            }
            if (!zValue.IsNone) {
                vector3Value.z = zValue.Value;
            }
            vector3Variable.Value = vector3Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector3Variable = UnityEngine.Vector3.zero;
            xValue = yValue = zValue = 0;
        }
    }
}