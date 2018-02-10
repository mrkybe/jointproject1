using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Behaviour
{
    [TaskCategory("Basic/Behaviour")]
    [TaskDescription("Stores the enabled state of the object. Returns Success.")]
    public class GetIsEnabled : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Object to use")]
        public SharedObject specifiedObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The enabled/disabled state")]
        [RequiredField]
        public SharedBool storeValue;

        public override TaskStatus OnUpdate()
        {
            if (specifiedObject == null && !(specifiedObject.Value is UnityEngine.Behaviour)) {
                UnityEngine.Debug.LogWarning("SpecifiedObject is null or not a subclass of UnityEngine.Behaviour");
                return TaskStatus.Failure;
            }

            storeValue.Value = (specifiedObject.Value as UnityEngine.Behaviour).enabled;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            if (specifiedObject != null) {
                specifiedObject.Value = null;
            }
            storeValue = false;
        }
    }
}