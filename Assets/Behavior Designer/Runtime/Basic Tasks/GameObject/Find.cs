using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.GameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Finds a GameObject by name. Returns Success.")]
    public class Find : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject name to find")]
        public SharedString gameObjectName;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The object found by name")]
        [RequiredField]
        public SharedGameObject storeValue;

        public override TaskStatus OnUpdate()
        {
            storeValue.Value = UnityEngine.GameObject.Find(gameObjectName.Value);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            gameObjectName = null;
            storeValue = null;
        }
    }
}