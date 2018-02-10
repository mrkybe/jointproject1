using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Math
{
    [TaskCategory("Basic/Math")]
    [TaskDescription("Sets a random bool value")]
    public class RandomBool : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The variable to store the result")]
        public SharedBool storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Random.value < 0.5f;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            storeResult.Value = false;
        }
    }
}