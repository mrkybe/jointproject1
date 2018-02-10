using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Classes.WorldSingleton;
using BehaviorDesigner.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomObjectDrawer(typeof(MarketOrder))]
    class CustomMarketOrderDrawer : ObjectDrawer
    {
        public override void OnGUI(GUIContent label)
        {
            base.OnGUI(label);
            MarketOrder marketOrder = value as MarketOrder;
            EditorGUILayout.BeginVertical();
            if (marketOrder != null && FieldInspector.DrawFoldout(marketOrder.GetHashCode(), label))
            {
                EditorGUI.indentLevel++;
                EditorGUI.TextArea(Rect.zero, "well");
                //marketOrder.destination = (Planet)EditorGUILayout.ObjectField("Object", marketOrder.destination, typeof(Planet), true);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }
    }
}
