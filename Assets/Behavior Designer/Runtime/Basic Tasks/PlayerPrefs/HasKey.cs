using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.PlayerPrefs
{
    [TaskCategory("Basic/PlayerPrefs")]
    [TaskDescription("Retruns success if the specified key exists.")]
    public class HasKey : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The key to check")]
        public SharedString key;

        public override TaskStatus OnUpdate()
        {
            return UnityEngine.PlayerPrefs.HasKey(key.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            key = "";
        }
    }
}