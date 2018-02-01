using System.Collections.Generic;
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

            if (myTarget.MyName != null)
            {
                EditorGUILayout.TextField("Name: ", myTarget.MyName);
            }

            if (myTarget.Faction != null)
            {
                EditorGUILayout.TextField("Faction: ", myTarget.Faction.Name);
            }

            if (myTarget.GetCargoHold != null)
            {
                hold = myTarget.GetCargoHold;
                
                string holdString = hold.ToString();
                string reserveString = myTarget.GetReserveCargoHold.ToString();


                EditorGUILayout.TextArea(holdString, GUILayout.MinHeight(40), GUILayout.MaxHeight(200), GUILayout.ExpandHeight(true));
                EditorGUILayout.TextArea(reserveString, GUILayout.MinHeight(40), GUILayout.MaxHeight(200), GUILayout.ExpandHeight(true));
                EditorGUILayout.TextArea(myTarget.BuildingsToString(), GUILayout.MinHeight(40), GUILayout.MaxHeight(120), GUILayout.ExpandHeight(true));
            }

            if (GUILayout.Button("Calculate Net Demand"))
            {
                myTarget.CalculateNetDemand();
            }

            if (GUILayout.Button("Spawn Gold Mining Ship"))
            {
                List<string> miningTargetList = new List<string>();
                miningTargetList.Add("Gold");
                Spaceship ship = myTarget.SpawnMiningShip(miningTargetList);
                Selection.objects = new[]{ship.gameObject};
            }

            base.OnInspectorGUI();
        }
    }
}