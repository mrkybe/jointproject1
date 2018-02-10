using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Gets the GameObject from the Transform component. Returns Success.")]
    public class SharedTransformToGameObject : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Transform component")]
        public SharedTransform sharedTransform;
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject to set")]
        public SharedGameObject sharedGameObject;

        public override TaskStatus OnUpdate()
        {
            if (sharedTransform.Value == null) {
                return TaskStatus.Failure;
            }

            sharedGameObject.Value = sharedTransform.Value.gameObject;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            sharedTransform = null;
            sharedGameObject = null;
        }
    }
}