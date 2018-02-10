using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Behavior_Designer.Runtime.Actions.Reflection
{
    [TaskDescription("Gets the value from the field specified. Returns success if the field was retrieved.")]
    [BehaviorDesigner.Runtime.Tasks.HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=147")]
    [TaskCategory("Reflection")]
    [TaskIcon("{SkinColor}ReflectionIcon.png")]
    public class GetFieldValue : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject to get the field on")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The component to get the field on")]
        public SharedString componentName;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The name of the field")]
        public SharedString fieldName;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The value of the field")]
        [RequiredField]
        public SharedVariable fieldValue;

        public override TaskStatus OnUpdate()
        {
            if (fieldValue == null) {
                Debug.LogWarning("Unable to get field - field value is null");
                return TaskStatus.Failure;
            }

            var type = TaskUtility.GetTypeWithinAssembly(componentName.Value);
            if (type == null) {
                Debug.LogWarning("Unable to get field - type is null");
                return TaskStatus.Failure;
            }

            var component = GetDefaultGameObject(targetGameObject.Value).GetComponent(type);
            if (component == null) {
                Debug.LogWarning("Unable to get the field with component " + componentName.Value);
                return TaskStatus.Failure;
            }

            // If you are receiving a compiler error on the Windows Store platform see this topic:
            // http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=46 
            var field = component.GetType().GetField(fieldName.Value);
            fieldValue.SetValue(field.GetValue(component));

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            componentName = null;
            fieldName = null; 
            fieldValue = null;
        }
    }
}