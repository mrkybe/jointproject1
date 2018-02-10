using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.LayerMask
{
    [TaskCategory("Basic/LayerMask")]
    [TaskDescription("Gets the layer of a GameObject.")]
    public class GetLayer : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject to set the layer of")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The name of the layer to get")]
        [RequiredField]
        public SharedString storeResult;

        public override TaskStatus OnUpdate()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            storeResult.Value = UnityEngine.LayerMask.LayerToName(currentGameObject.layer);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            storeResult = "";
        }
    }
}