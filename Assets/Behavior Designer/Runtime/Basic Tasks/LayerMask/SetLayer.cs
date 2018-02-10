using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.LayerMask
{
    [TaskCategory("Basic/LayerMask")]
    [TaskDescription("Sets the layer of a GameObject.")]
    public class SetLayer : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject to set the layer of")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The name of the layer to set")]
        public SharedString layerName = "Default";

        public override TaskStatus OnUpdate()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            currentGameObject.layer = UnityEngine.LayerMask.NameToLayer(layerName.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            layerName = "Default";
        }
    }
}