using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Classes.Helper.ShipInternals
{
    class BlackBoxEntry
    {
        public float Timestamp;
        public string Title;
        public string Message;
        public Vector3 GPS;
        public Vector3 LPS;

        BlackBoxEntry()
        {
            Timestamp = Time.time;
        }
    }

    class BlackBox : ScriptableObject
    {
        public string ShipName;
        public string CaptainName;
    }
}
