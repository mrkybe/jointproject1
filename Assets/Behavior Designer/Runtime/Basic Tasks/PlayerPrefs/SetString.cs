using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.PlayerPrefs
{
    [TaskCategory("Basic/PlayerPrefs")]
    [TaskDescription("Sets the value with the specified key from the PlayerPrefs.")]
    public class SetString : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The key to store")]
        public SharedString key;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The value to set")]
        public SharedString value;

        public override TaskStatus OnUpdate()
        {
            UnityEngine.PlayerPrefs.SetString(key.Value, value.Value);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            key = "";
            value = "";
        }
    }
}