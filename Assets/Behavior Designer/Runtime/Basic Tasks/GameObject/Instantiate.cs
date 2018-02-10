using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.GameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Instantiates a new GameObject. Returns Success.")]
    public class Instantiate : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The position of the new GameObject")]
        public SharedVector3 position;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The rotation of the new GameObject")]
        public SharedQuaternion rotation = UnityEngine.Quaternion.identity;
        [SharedRequired]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The instantiated GameObject")]
        public SharedGameObject storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.GameObject.Instantiate(targetGameObject.Value, position.Value, rotation.Value) as UnityEngine.GameObject;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            position = UnityEngine.Vector3.zero;
            rotation = UnityEngine.Quaternion.identity;
        }
    }
}