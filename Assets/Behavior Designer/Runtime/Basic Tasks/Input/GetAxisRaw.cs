using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Input
{
    [TaskCategory("Basic/Input")]
    [TaskDescription("Stores the raw value of the specified axis and stores it in a float.")]
    public class GetAxisRaw : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The name of the axis")]
        public SharedString axisName;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Axis values are in the range -1 to 1. Use the multiplier to set a larger range")]
        public SharedFloat multiplier;
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored result")]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            var axisValue = UnityEngine.Input.GetAxis(axisName.Value);

            // if variable set to none, assume multiplier of 1
            if (!multiplier.IsNone) {
                axisValue *= multiplier.Value;
            }

            storeResult.Value = axisValue;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            axisName = "";
            multiplier = 1.0f;
            storeResult = 0;
        }
    }
}