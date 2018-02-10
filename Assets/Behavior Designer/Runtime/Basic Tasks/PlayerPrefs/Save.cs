using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.PlayerPrefs
{
    [TaskCategory("Basic/PlayerPrefs")]
    [TaskDescription("Saves the PlayerPrefs.")]
    public class Save : Action
    {
        public override TaskStatus OnUpdate()
        {
            UnityEngine.PlayerPrefs.Save();

            return TaskStatus.Success;
        }
    }
}