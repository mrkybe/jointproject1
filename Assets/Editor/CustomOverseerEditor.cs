using Assets.Scripts.Classes.WorldSingleton;
using ShipInternals;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor {
    [CustomEditor(typeof(Overseer))]
    public class CustomOverseerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Overseer myTarget = (Overseer)target;

            if (GUILayout.Button("Match Orders"))
            {
                myTarget.MatchOrders();
            }

            base.OnInspectorGUI();
        }
    }
}