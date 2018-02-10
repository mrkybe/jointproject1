using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.String
{
    [TaskCategory("Basic/String")]
    [TaskDescription("Randomly selects a string from the array of strings.")]
    public class GetRandomString : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The array of strings")]
        public SharedString[] source;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored result")]
        [RequiredField]
        public SharedString storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = source[Random.Range(0, source.Length)].Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            source = null;
            storeResult = null;
        }
    }
}