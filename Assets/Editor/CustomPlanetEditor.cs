using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Classes.Helper.ShipInternals;
using Assets.Scripts.Classes.Mobile;
using Assets.Scripts.Classes.Static;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor {
    [CustomEditor(typeof(Planet))]
    public class CustomPlanetEditor : UnityEditor.Editor
    {
        private CargoHold hold;

        private GUIStyle style;

        public void OnEnable()
        {
            style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.font = Resources.Load<Font>("Fonts/VeraMono");
        }

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
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Spawn Gold Mining Ship"))
            {
                List<string> miningTargetList = new List<string>();
                miningTargetList.Add("Gold");
                myTarget.SpawnMiningShip(miningTargetList);
            }
            if (GUILayout.Button("and Select It"))
            {
                List<string> miningTargetList = new List<string>();
                miningTargetList.Add("Gold");
                Spaceship ship = myTarget.SpawnMiningShip(miningTargetList);
                Selection.objects = new[] { ship.gameObject };
            }
            GUILayout.EndHorizontal();

            base.OnInspectorGUI();
        }

        public void OnSceneGUI()
        {
            if (target)
            {
                Planet myTarget = target as Planet;
                if (myTarget)
                {
                    if (myTarget.GetCargoHold != null)
                    {
                        hold = myTarget.GetCargoHold;

                        string holdString = hold.ToString();
                        string reserveString = myTarget.GetReserveCargoHold.ToString();

                        string goodsValue = "";
                        string reserveGoodsValue = "";
                        Handles.color = Color.blue;
                        string s1 = holdString;
                        string s2 = reserveString;
                        string s3 = myTarget.BuildingsToString();
                        string s1x = "== Main Cargohold ======\n" + s1;


                        if (Application.isPlaying)
                        {
                            goodsValue = hold.GetMoneyValue().ToString();
                            reserveGoodsValue = myTarget.GetReserveCargoHold.GetMoneyValue().ToString(); ;
                        }

                        string s11 = "== Money : " + myTarget.Money + " | " + goodsValue + " | " + reserveGoodsValue;
                        string s2x = "== Reserved Cargohold ==\n" + s2;
                        string s3x = "== Buildings ===========\n" + s3;
                        Handles.Label(myTarget.transform.position + Vector3.up * -5, s11 + "\n" + s1x + "\n" + s2x + "\n" + s3x, style);
                    }
                }
            }
        }
    }
}