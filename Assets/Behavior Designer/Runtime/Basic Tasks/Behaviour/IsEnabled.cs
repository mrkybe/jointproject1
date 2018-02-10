using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Behaviour
{
    [TaskCategory("Basic/Behaviour")]
    [TaskDescription("Returns Success if the object is enabled, otherwise Failure.")]
    public class IsEnabled : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Object to use")]
        public SharedObject specifiedObject;

        public override TaskStatus OnUpdate()
        {
            if (specifiedObject == null && !(specifiedObject.Value is UnityEngine.Behaviour)) {
                UnityEngine.Debug.LogWarning("SpecifiedObject is null or not a subclass of UnityEngine.Behaviour");
                return TaskStatus.Failure;
            }

            return (specifiedObject.Value as UnityEngine.Behaviour).enabled ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            if (specifiedObject != null) {
                specifiedObject.Value = null;
            }
        }
    }
}