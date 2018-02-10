using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Physics2D
{
    [TaskCategory("Basic/Physics2D")]
    [TaskDescription("Returns success if there is any collider intersecting the line between start and end")]
    [BehaviorDesigner.Runtime.Tasks.HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=118")]
    public class Linecast : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The starting position of the linecast.")]
        SharedVector2 startPosition;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The ending position of the linecast.")]
        SharedVector2 endPosition;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Selectively ignore colliders.")]
        public UnityEngine.LayerMask layerMask = -1;

        public override TaskStatus OnUpdate()
        {
            return UnityEngine.Physics2D.Linecast(startPosition.Value, endPosition.Value, layerMask) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            startPosition = UnityEngine.Vector2.zero;
            endPosition = UnityEngine.Vector2.zero;
            layerMask = -1;
        }
    }
}
