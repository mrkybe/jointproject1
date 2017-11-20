using ShipInternals;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor {
    [CustomEditor(typeof(Spaceship))]
    public class CustomSpaceshipEditor : UnityEditor.Editor
    {
        private CargoHold hold;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Spaceship myTarget = (Spaceship)target;
            hold = myTarget.GetCargoHold;

            if (hold != null)
            {
                EditorGUILayout.TextArea(hold.ToString(), GUILayout.MaxHeight(75));
            }

            base.OnInspectorGUI();
        }
    }
}