using ShipInternals;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor {
    [CustomEditor(typeof(Planet))]
    public class CustomPlanetEditor : UnityEditor.Editor
    {
        private CargoHold hold;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Planet myTarget = (Planet)target;
            hold = myTarget.GetCargoHold;

            if (hold != null)
            {
                string holdString = hold.ToString();

                EditorGUILayout.TextArea(holdString, GUILayout.MinHeight(40), GUILayout.MaxHeight(200), GUILayout.ExpandHeight(true));
                EditorGUILayout.TextArea(myTarget.BuildingsToString(), GUILayout.MinHeight(40), GUILayout.MaxHeight(120), GUILayout.ExpandHeight(true));
            }

            base.OnInspectorGUI();
        }
    }
}