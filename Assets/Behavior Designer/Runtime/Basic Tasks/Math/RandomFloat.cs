using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Math
{
    [TaskCategory("Basic/Math")]
    [TaskDescription("Sets a random float value")]
    public class RandomFloat : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The minimum amount")]
        public SharedFloat min;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The maximum amount")]
        public SharedFloat max;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Is the maximum value inclusive?")]
        public bool inclusive;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The variable to store the result")]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            if (inclusive) {
                storeResult.Value = Random.Range(min.Value, max.Value);
            } else {
                storeResult.Value = Random.Range(min.Value, max.Value - 0.00001f);
            }
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            min.Value = 0;
            max.Value = 0;
            inclusive = false;
            storeResult.Value = 0;
        }
    }
}