using Assets.Behavior_Designer.Runtime.Object_Drawers;
using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Behavior_Designer.Editor.Object_Drawers
{
    [CustomObjectDrawer(typeof(FloatSliderAttribute))]
    public class FloatSliderDrawer : ObjectDrawer
    {
        public override void OnGUI(GUIContent label)
        {
            var floatSliderAttribute = (FloatSliderAttribute)attribute;
            if (value is SharedFloat) {
                var sharedFloat = value as SharedFloat;
                sharedFloat.Value = EditorGUILayout.Slider(label, sharedFloat.Value, floatSliderAttribute.min, floatSliderAttribute.max);
            } else {
                value = EditorGUILayout.Slider(label, (float)value, floatSliderAttribute.min, floatSliderAttribute.max);
            }
        }
    }
}