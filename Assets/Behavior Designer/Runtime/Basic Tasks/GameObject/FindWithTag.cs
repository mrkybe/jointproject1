using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.GameObject
{
    [TaskCategory("Basic/GameObject")]
    [TaskDescription("Finds a GameObject by tag. Returns Success.")]
    public class FindWithTag : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The tag of the GameObject to find")]
        public SharedString tag;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The object found by name")]
        [RequiredField]
        public SharedGameObject storeValue;

        public override TaskStatus OnUpdate()
        {
            storeValue.Value = UnityEngine.GameObject.FindWithTag(tag.Value);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            tag.Value = null;
            storeValue.Value = null;
        }
    }
}