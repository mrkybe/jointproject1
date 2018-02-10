using BehaviorDesigner.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Classes.WorldSingleton;
using ShipInternals;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
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
