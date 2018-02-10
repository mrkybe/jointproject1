using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Input
{
    [TaskCategory("Basic/Input")]
    [TaskDescription("Stores the pressed state of the specified key.")]
    public class GetKey : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The key to test.")]
        public KeyCode key;
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The stored result")]
        public SharedBool storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = UnityEngine.Input.GetKey(key);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            key = KeyCode.None;
            storeResult = false;
        }
    }
}