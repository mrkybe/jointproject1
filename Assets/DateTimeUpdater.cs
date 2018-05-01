using System;
using Assets.Scripts.Classes.WorldSingleton;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class DateTimeUpdater : MonoBehaviour
    {
        private Text text;

        //private DateTime time;
        // Use this for initialization
        void Start ()
        {
            text = this.GetComponent<Text>();
            //time = Overseer.Main.InUniverseDateTime;
            InvokeRepeating("UpdateDateTime", 1, 0.75f);
        }

        void UpdateDateTime()
        {
            DateTime
                time = Overseer.Main.InUniverseDateTime;
            text.text = time.ToLongDateString() + "\n" +
                        time.ToShortTimeString();
        }
    }
}
