using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Light
{
    [TaskCategory("Basic/Light")]
    [TaskDescription("Stores the light's cookie size.")]
    public class GetCookieSize : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The size to store")]
        public SharedFloat storeValue;

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

            storeValue = light.cookieSize;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            storeValue = 0;
        }
    }
}
