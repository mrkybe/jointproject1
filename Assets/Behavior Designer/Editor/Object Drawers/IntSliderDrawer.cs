using Assets.Behavior_Designer.Runtime.Object_Drawers;
using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Behavior_Designer.Editor.Object_Drawers
{
    [CustomObjectDrawer(typeof(IntSliderAttribute))]
    public class IntSliderDrawer : ObjectDrawer
    {
        public override void OnGUI(GUIContent label)
        {
            var intSliderAttribute = (IntSliderAttribute)attribute;
            if (value is SharedInt) {
                var sharedFloat = value as SharedInt;
                sharedFloat.Value = EditorGUILayout.IntSlider(label, sharedFloat.Value, intSliderAttribute.min, intSliderAttribute.max);
            } else {
                value = EditorGUILayout.IntSlider(label, (int)value, intSliderAttribute.min, intSliderAttribute.max);
            }
        }
    }
}