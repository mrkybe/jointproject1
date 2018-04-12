using Assets.Scripts.Classes.Helper.Pilot;
using Assets.Scripts.Classes.Helper;
using Assets.Scripts.Classes.Helper.ShipInternals;
using Assets.Scripts.Classes.Mobile;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor {
    [CustomEditor(typeof(NameGenerator))]
    public class CustomNameGeneratorEditor : UnityEditor.Editor
    {
        private CargoHold hold;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            NameGenerator myTarget = (NameGenerator)target;

            if(myTarget != null)
            {
                if (Application.isPlaying)
                {
                    if (GUILayout.Button("Random Name"))
                    {
                        Debug.Log(myTarget.RandomName());
                    }
                    if (GUILayout.Button("Random Person"))
                    {
                        Debug.Log(myTarget.RandomPerson());
                    }
                }
            }

            base.OnInspectorGUI();
        }
    }
}