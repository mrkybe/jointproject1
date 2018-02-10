using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.PlayerPrefs
{
    [TaskCategory("Basic/PlayerPrefs")]
    [TaskDescription("Stores the value with the specified key from the PlayerPrefs.")]
    public class GetString : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The key to store")]
        public SharedString key;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The default value")]
        public SharedString defaultValue;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The value retrieved from the PlayerPrefs")]
        [RequiredField]
        public SharedString storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.PlayerPrefs.GetString(key.Value, defaultValue.Value);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            key = "";
            defaultValue = "";
            storeResult = "";
        }
    }
}