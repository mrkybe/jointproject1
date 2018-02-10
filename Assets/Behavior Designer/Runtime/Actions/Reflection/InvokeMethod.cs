using System;
using System.Collections.Generic;
using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Behavior_Designer.Runtime.Actions.Reflection
{
    [TaskDescription("Invokes the specified method with the specified parameters. Can optionally store the return value. Returns success if the method was invoked.")]
    [BehaviorDesigner.Runtime.Tasks.HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=145")]
    [TaskCategory("Reflection")]
    [TaskIcon("{SkinColor}ReflectionIcon.png")]
    public class InvokeMethod : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject to invoke the method on")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The component to invoke the method on")]
        public SharedString componentName;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The name of the method")]
        public SharedString methodName;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The first parameter of the method")]
        public SharedVariable parameter1;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The second parameter of the method")]
        public SharedVariable parameter2;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The third parameter of the method")]
        public SharedVariable parameter3;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The fourth parameter of the method")]
        public SharedVariable parameter4;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Store the result of the invoke call")]
        public SharedVariable storeResult;

        public override TaskStatus OnUpdate()
        {
            var type = TaskUtility.GetTypeWithinAssembly(componentName.Value);
            if (type == null) {
                Debug.LogWarning("Unable to invoke - type is null");
                return TaskStatus.Failure;
            }

            var component = GetDefaultGameObject(targetGameObject.Value).GetComponent(type);
            if (component == null) {
                Debug.LogWarning("Unable to invoke method with component " + componentName.Value);
                return TaskStatus.Failure;
            }

            var parameterList = new List<object>();
            var parameterTypeList = new List<Type>();
            SharedVariable sharedVariable = null;
            for (int i = 0; i < 4; ++i) {
                var parameterField = GetType().GetField("parameter" + (i + 1));
                if ((sharedVariable = parameterField.GetValue(this) as SharedVariable) != null) {
                    parameterList.Add(sharedVariable.GetValue());
                    parameterTypeList.Add(sharedVariable.GetType().GetProperty("Value").PropertyType);
                } else {
                    break;
                }
            }
            // If you are receiving a compiler error on the Windows Store platform see this topic:
            // http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=46 
            var methodInfo = component.GetType().GetMethod(methodName.Value, parameterTypeList.ToArray());

            if (methodInfo == null) {
                Debug.LogWarning("Unable to invoke method " + methodName.Value + " on component " + componentName.Value);
                return TaskStatus.Failure;
            }

            var result = methodInfo.Invoke(component, parameterList.ToArray());
            if (storeResult != null) {
                storeResult.SetValue(result);
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            componentName = null;
            methodName = null; 
            parameter1 = null;
            parameter2 = null;
            parameter3 = null;
            parameter4 = null;
            storeResult = null;
        }
    }
}