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
            PilotInterface pilot = myTarget.GetPilot;
            hold = myTarget.GetCargoHold;

            if(pilot != null)
            {
                if (pilot.GetType() == typeof(AI_Patrol))
                {
                    if (GUILayout.Button("Become Pirate"))
                    {
                        ((AI_Patrol)pilot).StartPirate();
                        myTarget.PowerLevel = 5;
                    }
                }
            }

            if (hold != null)
            {
                EditorGUILayout.TextArea(hold.ToString(), GUILayout.MaxHeight(75));
            }

            base.OnInspectorGUI();
        }
    }
}