using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Classes.Helper.ShipInternals
{
    /// <summary>
    /// One time-stamped entry in the Blackbox recorder of a ship.
    /// </summary>
    class BlackBoxEntry
    {
        /// <summary>
        /// The timestamp associated with this entry.
        /// </summary>
        public float Timestamp;

        /// <summary>
        /// The title of this entry, may be shown to the player.
        /// </summary>
        public string Title;

        /// <summary>
        /// The text explaining this entry, may be shown to player.
        /// </summary>
        public string Message;

        /// <summary>
        /// Global Positioning System.  Ship location in Overmap at time of entry.
        /// </summary>
        public Vector3 GPS;

        /// <summary>
        /// Local Positioning System.  Ship location in Combat map at time of entry.
        /// </summary>
        public Vector3 LPS;

        BlackBoxEntry()
        {
            Timestamp = Time.time;
        }
    }
}
