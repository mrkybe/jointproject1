using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Physics
{
    [TaskCategory("Basic/Physics")]
    [TaskDescription("Returns success if there is any collider intersecting the line between start and end")]
    [BehaviorDesigner.Runtime.Tasks.HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=117")]
    public class Linecast : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The starting position of the linecast")]
        SharedVector3 startPosition;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The ending position of the linecast")]
        SharedVector3 endPosition;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Selectively ignore colliders.")]
        public UnityEngine.LayerMask layerMask = -1;

        public override TaskStatus OnUpdate()
        {
            return UnityEngine.Physics.Linecast(startPosition.Value, endPosition.Value, layerMask) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            startPosition = UnityEngine.Vector3.zero;
            endPosition = UnityEngine.Vector3.zero;
            layerMask = -1;
        }
    }
}