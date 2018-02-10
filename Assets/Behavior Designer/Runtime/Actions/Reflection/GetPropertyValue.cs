using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Behavior_Designer.Runtime.Actions.Reflection
{
    [TaskDescription("Gets the value from the property specified. Returns success if the property was retrieved.")]
    [BehaviorDesigner.Runtime.Tasks.HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=148")]
    [TaskCategory("Reflection")]
    [TaskIcon("{SkinColor}ReflectionIcon.png")]
    public class GetPropertyValue : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject to get the property of")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The component to get the property of")]
        public SharedString componentName;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The name of the property")]
        public SharedString propertyName;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The value of the property")]
        [RequiredField]
        public SharedVariable propertyValue;

        public override TaskStatus OnUpdate()
        {
            if (propertyValue == null) {
                Debug.LogWarning("Unable to get property - property value is null");
                return TaskStatus.Failure;
            }
            
            var type = TaskUtility.GetTypeWithinAssembly(componentName.Value);
            if (type == null) {
                Debug.LogWarning("Unable to get property - type is null");
                return TaskStatus.Failure;
            }

            var component = GetDefaultGameObject(targetGameObject.Value).GetComponent(type);
            if (component == null) {
                Debug.LogWarning("Unable to get the property with component " + componentName.Value);
                return TaskStatus.Failure;
            }

            // If you are receiving a compiler error on the Windows Store platform see this topic:
            // http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=46 
            var property = component.GetType().GetProperty(propertyName.Value);
            propertyValue.SetValue(property.GetValue(component, null));

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            componentName = null;
            propertyName = null;
            propertyValue = null;
        }
    }
}