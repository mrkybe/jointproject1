using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Stores the X and Y values of the Vector2.")]
    public class GetXY : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Vector2 to get the values of")]
        public SharedVector2 vector2Variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The X value")]
        [RequiredField]
        public SharedFloat storeX;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Y value")]
        [RequiredField]
        public SharedFloat storeY;

        public override TaskStatus OnUpdate()
        {
            storeX.Value = vector2Variable.Value.x;
            storeY.Value = vector2Variable.Value.y;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            vector2Variable = UnityEngine.Vector2.zero;
            storeX = storeY = 0;
        }
    }
}