using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Gets the Transform from the GameObject. Returns Success.")]
    public class SharedGameObjectToTransform : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject to get the Transform of")]
        public SharedGameObject sharedGameObject;
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Transform to set")]
        public SharedTransform sharedTransform;

        public override TaskStatus OnUpdate()
        {
            if (sharedGameObject.Value == null) {
                return TaskStatus.Failure;
            }

            sharedTransform.Value = sharedGameObject.Value.GetComponent<UnityEngine.Transform>();

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            sharedGameObject = null;
            sharedTransform = null;
        }
    }
}