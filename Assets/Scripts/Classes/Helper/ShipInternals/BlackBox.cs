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

        public BlackBoxEntry()
        {
            Timestamp = Time.time;
        }
    }

    /// <summary>
    /// Captain's log.
    /// </summary>
    public class BlackBox
    {
        public string ShipName;

        private List<BlackBoxEntry> entries;

        public BlackBox()
        {
            entries = new List<BlackBoxEntry>();
        }

        public void Write(string message, Vector3 GPS, Vector3 LPS = new Vector3())
        {
            BlackBoxEntry newEntry = new BlackBoxEntry
            {
                GPS = GPS,
                LPS = LPS,
                Message = message
            };
            entries.Add(newEntry);
        }
    }
}
