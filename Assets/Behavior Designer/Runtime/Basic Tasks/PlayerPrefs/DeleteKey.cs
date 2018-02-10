using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.PlayerPrefs
{
    [TaskCategory("Basic/PlayerPrefs")]
    [TaskDescription("Deletes the specified key from the PlayerPrefs.")]
    public class DeleteKey : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The key to delete")]
        public SharedString key;

        public override TaskStatus OnUpdate()
        {
            UnityEngine.PlayerPrefs.DeleteKey(key.Value);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            key = "";
        }
    }
}