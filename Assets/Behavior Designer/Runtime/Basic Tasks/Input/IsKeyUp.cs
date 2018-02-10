using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Input
{
    [TaskCategory("Basic/Input")]
    [TaskDescription("Returns success when the specified key is released.")]
    public class IsKeyUp : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The key to test")]
        public KeyCode key;

        public override TaskStatus OnUpdate()
        {
            return UnityEngine.Input.GetKeyUp(key) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            key = KeyCode.None;
        }
    }
}