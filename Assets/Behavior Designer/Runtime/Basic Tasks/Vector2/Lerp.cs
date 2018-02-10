using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Lerp the Vector2 by an amount.")]
    public class Lerp : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The from value")]
        public SharedVector2 fromVector2;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The to value")]
        public SharedVector2 toVector2;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The amount to lerp")]
        public SharedFloat lerpAmount;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The lerp resut")]
        [RequiredField]
        public SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Vector2.Lerp(fromVector2.Value, toVector2.Value, lerpAmount.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            fromVector2 = toVector2 = storeResult = UnityEngine.Vector2.zero;
            lerpAmount = 0;
        }
    }
}