using ShipInternals;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor {
    [CustomEditor(typeof(AsteroidField))]
    public class CustomAsteroidFieldEditor : UnityEditor.Editor
    {
        private CargoHold hold;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            AsteroidField myTarget = (AsteroidField)target;
            hold = myTarget.CargoHold;

            if (hold != null)
            {
                EditorGUILayout.TextArea(hold.ToString(), GUILayout.MaxHeight(200));
            }

            base.OnInspectorGUI();
        }
    }
}