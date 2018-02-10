using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Conditionals.Reflection
{
    [TaskDescription("Compares the field value to the value specified. Returns success if the values are the same.")]
    [BehaviorDesigner.Runtime.Tasks.HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=151")]
    [TaskCategory("Reflection")]
    [TaskIcon("{SkinColor}ReflectionIcon.png")]
    public class CompareFieldValue : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject to compare the field on")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The component to compare the field on")]
        public SharedString componentName;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The name of the field")]
        public SharedString fieldName;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The value to compare to")]
        public SharedVariable compareValue;

        public override TaskStatus OnUpdate()
        {
            if (compareValue == null) {
                Debug.LogWarning("Unable to compare field - compare value is null");
                return TaskStatus.Failure;
            }

            var type = TaskUtility.GetTypeWithinAssembly(componentName.Value);
            if (type == null) {
                Debug.LogWarning("Unable to compare field - type is null");
                return TaskStatus.Failure;
            }

            var component = GetDefaultGameObject(targetGameObject.Value).GetComponent(type);
            if (component == null) {
                Debug.LogWarning("Unable to compare the field with component " + componentName.Value);
                return TaskStatus.Failure;
            }

            // If you are receiving a compiler error on the Windows Store platform see this topic:
            // http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=46 
            var field = component.GetType().GetField(fieldName.Value);
            var fieldValue = field.GetValue(component);

            if (fieldValue == null && compareValue.GetValue() == null) {
                return TaskStatus.Success;
            }

            return fieldValue.Equals(compareValue.GetValue()) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            componentName = null;
            fieldName = null;
            compareValue = null;
        }
    }
}