using System;
using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.String
{
    [TaskCategory("Basic/String")]
    [TaskDescription("Stores a string with the specified format.")]
    public class Format : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The format of the string")]
        public SharedString format;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Any variables to appear in the string")]
        public SharedGenericVariable[] variables;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The result of the format")]
        [RequiredField]
        public SharedString storeResult;

        private object[] variableValues;

        public override void OnAwake()
        {
            variableValues = new object[variables.Length];
        }

        public override TaskStatus OnUpdate()
        {
            for (int i = 0; i < variableValues.Length; ++i) {
                variableValues[i] = variables[i].Value.value.GetValue();
            }

            try {
                storeResult.Value = string.Format(format.Value, variableValues);
            } catch (Exception e) {
                UnityEngine.Debug.LogError(e.Message);
                return TaskStatus.Failure;
            }
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            format = "";
            variables = null;
            storeResult = null;
        }
    }
}