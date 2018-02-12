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
                        string s1  = "Throttle    : ";
                        string s1d = "Throttle    : ";
                        string s2  = "TargetSpeed : ";
                        string s2d = "TargetSpeed : ";
                        string s3  = "Speed       : ";
                        string s3d = "Speed       : ";
                        string s1x = myTarget.Throttle          < 0 ? s1 : s1d;
                        string s2x = myTarget.TargetSpeed.Value < 0 ? s2 : s2d;
                        string s3x = myTarget.Speed             < 0 ? s3 : s3d;
                        s1x += string.Format("{0:0.00}", myTarget.Throttle);
                        s2x += string.Format("{0:0.00}", myTarget.TargetSpeed.Value);
                        s3x += string.Format("{0:0.00}", myTarget.Speed);
                        string holdString = "";
                        if (mySpaceship != null)
                        {
                            hold = mySpaceship.GetCargoHold;
                            if (hold != null)
                            {
                                holdString = "== Main Cargohold ======\n" + hold.ToString();
                            }
                        }
                        Handles.Label(myTarget.transform.position + Vector3.up * -2f, s1x + "\n" + s2x + "\n" + s3x + "\n" + holdString, style);
                    }
                }
            }
        }
    }
}
