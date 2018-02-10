using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Classes.Helper.ShipInternals
{
    /// <summary>
    /// One time-stamped entry in a Blackbox recorder.
    /// </summary>
    class BlackBoxEntry
    {
        public float Timestamp;
        public string Title;
        public string Message;
        // Ship location in Overmap at time of entry.
        public Vector3 GPS = Vector3.zero;
        // Ship location in Combat map at time of entry.
        public Vector3 LPS = Vector3.zero;

        BlackBoxEntry()
        {
            Timestamp = Time.time;
        }
    }

    /// <summary>
    /// Captain's log.
    /// </summary>
    class BlackBox : ScriptableObject
    {
        public string ShipName;
        public string CaptainName;
    }
}
