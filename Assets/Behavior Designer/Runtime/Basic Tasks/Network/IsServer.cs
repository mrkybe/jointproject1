using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.Networking;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Network
{
    public class IsServer : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            return NetworkServer.active ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}