using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Classes.Helper.Pilot;
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

            public void OnEnable()
            {
                style = new GUIStyle();
                style.normal.textColor = Color.white;
            }

            public void OnSceneGUI()
            {
                if (target)
                {
                    AI_Patrol tgt = target as AI_Patrol;
                    if (tgt)
                    {
                        Handles.color = Color.blue;
                        string s1 = "Throttle:";
                        string s2 = "TargetSpeed:";
                        string s3 = "Speed:";
                        string s1x = s1 + string.Format("{0:0.00}", tgt.Throttle);
                        string s2x = s2 + string.Format("{0:0.00}", tgt.TargetSpeed);
                        string s3x = s3 + string.Format("{0:0.00}", tgt.Speed);
                        Handles.Label(tgt.transform.position + Vector3.up * 5, s1x + "\n" + s2x + "\n" + s3x, style);
                    }
                }
            }
        }
    }
}
