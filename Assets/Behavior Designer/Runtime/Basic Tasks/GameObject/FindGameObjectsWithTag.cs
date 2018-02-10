using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.GameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Finds a GameObject by tag. Returns Success.")]
    public class FindGameObjectsWithTag : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The tag of the GameObject to find")]
        public SharedString tag;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The objects found by name")]
        [RequiredField]
        public SharedGameObjectList storeValue;

        public override TaskStatus OnUpdate()
        {
            var gameObjects = UnityEngine.GameObject.FindGameObjectsWithTag(tag.Value);
            for (int i = 0; i < gameObjects.Length; ++i) {
                storeValue.Value.Add(gameObjects[i]);
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            tag.Value = null;
            storeValue.Value = null;
        }
    }
}