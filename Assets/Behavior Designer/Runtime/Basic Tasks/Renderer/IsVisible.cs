using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Renderer
{
    [TaskCategory("Basic/Renderer")]
    [TaskDescription("Returns Success if the Renderer is visible, otherwise Failure.")]
    public class IsVisible : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;

        // cache the renderer component
        private UnityEngine.Renderer renderer;
        private UnityEngine.GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                renderer = currentGameObject.GetComponent<UnityEngine.Renderer>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (renderer == null) {
                UnityEngine.Debug.LogWarning("Renderer is null");
                return TaskStatus.Failure;
            }

            return renderer.isVisible ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            targetGameObject = null;
        }
    }
}