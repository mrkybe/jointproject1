using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Light
{
    [TaskCategory("Basic/Light")]
    [TaskDescription("Sets the culling mask of the light.")]
    public class SetCullingMask : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The culling mask to set")]
        public UnityEngine.LayerMask cullingMask;

        // cache the light component
        private UnityEngine.Light light;
        private UnityEngine.GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                light = currentGameObject.GetComponent<UnityEngine.Light>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (light == null) {
                UnityEngine.Debug.LogWarning("Light is null");
                return TaskStatus.Failure;
            }

            light.cullingMask = cullingMask.value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            cullingMask = -1;
        }
    }
}