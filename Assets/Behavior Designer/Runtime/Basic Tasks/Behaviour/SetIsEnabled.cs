using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Behaviour
{
    [TaskCategory("Basic/Behaviour")]
    [TaskDescription("Enables/Disables the object. Returns Success.")]
    public class SetIsEnabled : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Object to use")]
        public SharedObject specifiedObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The enabled/disabled state")]
        public SharedBool enabled;

        public override TaskStatus OnUpdate()
        {
            if (specifiedObject == null && !(specifiedObject.Value is UnityEngine.Behaviour)) {
                UnityEngine.Debug.LogWarning("SpecifiedObject is null or not a subclass of UnityEngine.Behaviour");
                return TaskStatus.Failure;
            }

            (specifiedObject.Value as UnityEngine.Behaviour).enabled = enabled.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            if (specifiedObject != null) {
                specifiedObject.Value = null;
            }
            enabled = false;
        }
    }
}