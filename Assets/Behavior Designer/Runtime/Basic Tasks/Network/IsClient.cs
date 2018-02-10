using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.Networking;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Network
{
    public class IsClient : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            return NetworkClient.active ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}