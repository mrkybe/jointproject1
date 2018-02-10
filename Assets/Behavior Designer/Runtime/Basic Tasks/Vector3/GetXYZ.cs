using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector3
{
    [TaskCategory("Basic/Vector3")]
    [TaskDescription("Stores the X, Y, and Z values of the Vector3.")]
    public class GetXYZ : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector3 to get the values of")]
        public SharedVector3 vector3Variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The X value")]
        [RequiredField]
        public SharedFloat storeX;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Y value")]
        [RequiredField]
        public SharedFloat storeY;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Z value")]
        [RequiredField]
        public SharedFloat storeZ;

        public override TaskStatus OnUpdate()
        {
            storeX.Value = vector3Variable.Value.x;
            storeY.Value = vector3Variable.Value.y;
            storeZ.Value = vector3Variable.Value.z;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector3Variable = UnityEngine.Vector3.zero;
            storeX = storeY = storeZ = 0;
        }
    }
}