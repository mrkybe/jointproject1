using Assets.Scripts.Classes.Helper.ShipInternals;
using BehaviorDesigner.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Behavior_Designer.Editor.Object_Drawers
{
    [CustomObjectDrawer(typeof(CargoItem))]
    class CustomCargoItemDrawer : ObjectDrawer
    {
        public override void OnGUI(GUIContent label)
        {
            base.OnGUI(label);
            CargoItem item = value as CargoItem;
            EditorGUILayout.BeginVertical();
            if (item != null && FieldInspector.DrawFoldout(item.GetHashCode(), label))
            {
                EditorGUI.indentLevel++;
                EditorGUI.TextArea(Rect.MinMaxRect(10,10,40,40), "well");
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }
    }
}
