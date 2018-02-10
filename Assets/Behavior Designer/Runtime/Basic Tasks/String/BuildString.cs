using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.String
{
    [TaskCategory("Basic/String")]
    [TaskDescription("Creates a string from multiple other strings.")]
    public class BuildString : Action
    {
        [Tooltip("The array of strings")]
        public SharedString[] source;
        [Tooltip("The stored result")]
        [RequiredField]
        public SharedString storeResult;

        public override TaskStatus OnUpdate()
        {
            for (int i = 0; i < source.Length; ++i) {
                storeResult.Value += source[i];
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            source = null;
            storeResult = null;
        }
    }
}