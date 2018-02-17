using Assets.Scripts.Classes.Helper.Pilot;
using Assets.Scripts.Classes.Helper.ShipInternals;
using Assets.Scripts.Classes.Mobile;
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
            PilotInterface pilot = myTarget.Pilot;
            hold = myTarget.GetCargoHold;

            if(pilot != null)
            {
                if (pilot.GetType() == typeof(AI_Patrol))
                {
                    if (GUILayout.Button("Become Pirate"))
                    {
                        ((AI_Patrol)pilot).StartPirate();
                        myTarget.PowerLevel = 150;
                    }
                    if (GUILayout.Button("Become Scrapper"))
                    {
                        ((AI_Patrol)pilot).StartScrapper();
                    }
                }
            }
            if (pilot != null)
            {
                if (GUILayout.Button("Take 100 Damage"))
                {
                    myTarget.TakeDamage(100);
                }
            }
            if (pilot != null)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Sensor Range + 5"))
                {
                    myTarget.SensorRange += 5;
                }
                if (GUILayout.Button("Sensor Range - 5"))
                {
                    myTarget.SensorRange -= 5;
                }
                GUILayout.EndHorizontal();
            }
            if (hold != null)
            {
                EditorGUILayout.TextArea(hold.ToString(), GUILayout.MaxHeight(75));
            }

            base.OnInspectorGUI();
        }
    }
}