using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Actions
{
    [TaskDescription("Log is a simple task which will output the specified text and return success. It can be used for debugging.")]
    [BehaviorDesigner.Runtime.Tasks.HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=16")]
    [TaskIcon("{SkinColor}LogIcon.png")]
    public class Log : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Text to output to the log")]
        public SharedString text;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Is this text an error?")]
        public SharedBool logError;
        
        public override TaskStatus OnUpdate()
        {
            // Log the text and return success
            if (logError.Value) {
                Debug.LogError(text);
            } else {
                Debug.Log(text);
            }
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            // Reset the properties back to their original values
            text = "";
            logError = false;
        }
    }
}