using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Classes.Helper.Pilot;
using Assets.Scripts.Classes.Helper.ShipInternals;
using Assets.Scripts.Classes.Mobile;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    class CustomAI_PatrolEditor
    {
        [CustomEditor(typeof(AI_Patrol))]
        public class CustomSpaceshipEditor : UnityEditor.Editor
        {
            private GUIStyle style;
            private CargoHold hold;
            private Spaceship mySpaceship;

            public void OnEnable()
            {
                style = new GUIStyle();
                style.normal.textColor = Color.white;
                style.font = Resources.Load<Font>("Fonts/VeraMono");
            }

            public void OnSceneGUI()
            {
                if (target)
                {
                    AI_Patrol myTarget = target as AI_Patrol;
                    mySpaceship = myTarget.GetComponent<Spaceship>();
                    if (myTarget)
                    {
                        Handles.color = Color.blue;
                        string sx = "";
                        if (myTarget.Faction != null)
                        {
                            sx = myTarget.Faction.Name + "\n" + myTarget.gameObject.name;
                        }
                        string s1  = sx + "\nThrottle    : ";
                        string s2  = "TargetSpeed : ";
                        string s3  = "Speed       : ";
                        string s4  = "Hull        : ";

                        s1 += string.Format("{0:0.00}", myTarget.Throttle);
                        s2 += string.Format("{0:0.00}", myTarget.TargetSpeed.Value);
                        s3 += string.Format("{0:0.00}", myTarget.Speed);
                        s4 += string.Format("{0:0.00}", mySpaceship.HullHealth);
                        string holdString = "";
                        if (mySpaceship != null)
                        {
                            hold = mySpaceship.GetCargoHold;
                            if (hold != null)
                            {
                                holdString = "== Main Cargohold ======\n" + hold.ToString();
                            }
                        }
                        Handles.Label(myTarget.transform.position + Vector3.up * -2f, s1 + "\n" + s2 + "\n" + s3 + "\n" + s4 + "\n" + holdString, style);
                    }
                }
            }
        }
    }
}
