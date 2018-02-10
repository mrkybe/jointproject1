using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.GameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Destorys the specified GameObject. Returns Success.")]
    public class Destroy : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Time to destroy the GameObject in")]
        public float time;

        public override TaskStatus OnUpdate()
        {
            var destroyGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (time == 0) {
                UnityEngine.GameObject.Destroy(destroyGameObject);
            } else {
                UnityEngine.GameObject.Destroy(destroyGameObject, time);
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            time = 0;
        }
    }
}