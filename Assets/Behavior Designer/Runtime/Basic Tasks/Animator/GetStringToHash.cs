using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Animator
{
    [TaskCategory("Basic/Animator")]
    [TaskDescription("Converts the state name to its corresponding hash code. Returns Success.")]
    public class GetStringToHash : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The name of the state to convert to a hash code")]
        public SharedString stateName;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The hash value")]
        [RequiredField]
        public SharedInt storeValue;

        public override TaskStatus OnUpdate()
        {
            storeValue.Value = UnityEngine.Animator.StringToHash(stateName.Value);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            stateName = "";
            storeValue = 0;
        }
    }
}