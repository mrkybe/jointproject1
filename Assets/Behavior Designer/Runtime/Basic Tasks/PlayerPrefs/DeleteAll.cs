using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.PlayerPrefs
{
    [TaskCategory("Basic/PlayerPrefs")]
    [TaskDescription("Deletes all entries from the PlayerPrefs.")]
    public class DeleteAll : Action
    {
        public override TaskStatus OnUpdate()
        {
            UnityEngine.PlayerPrefs.DeleteAll();

            return TaskStatus.Success;
        }
    }
}