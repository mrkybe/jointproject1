namespace ShipInternals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    /* How Cargo should work in theory:
         * 1.  I have a class that needs to store Cargo
         * 2.  I attach a CargoHold to that class
         * 3.  I set up the CargoBays
         *     a. CargoBays keep track of the amount of CargoItem they have in them.
         *          a1.  CargoBays do not actually store instances of CargoItem.
         *     b. CargoHold contains multiple CargoBays.
         */


    public class CargoHold
    {
        private int _maxHold;
        private List<CargoItem> _cargoItems;

        // CargoHolds should always be declared with the maximum amount of space they contain in units.
        public CargoHold(int maxHold_in)
        {
            _maxHold = maxHold_in;
            _cargoItems = new List<CargoItem>();
        }

        public void addHoldType(String type)
        {
            // Check that CargoItems of this type exist.  Or don't.  I'm a comment. Not a cop.
            if (true)
            {
                _cargoItems.Add(new CargoItem(type));
            }
            else
            {
                Debug.Log("-WARNING: could not addHoldType(" + type + ") as it does not exist!");
            }
        }

        public void addToHold(String type, int count)
        {
            // Check that CargoItems of this type exist.
            if (this.Contains(type))
            {
                for (int i = 0; i < _cargoItems.Count; i++)
                {
                    if (_cargoItems[i].Name == type)
                    {
                        Debug.Log("In Hold!: " + _cargoItems[i].Count + " adding " + count);
                        _cargoItems[i].Count = count;
                    }
                }
            }
            else
            {
                Debug.Log("-WARNING: could not addToHold(" + type + ") as it does not exist in this instace!");
            }
        }

        public bool Contains(String type)
        {
            bool contains = false;
            for (int i = 0; i < _cargoItems.Count; i++)
            {
                if (_cargoItems[i].Name == type)
                {
                    contains = true;
                }
            }
            return contains;
        }

        public int getTotalHold()
        {
            int total = 0;
            foreach (CargoItem item in _cargoItems)
            {
                total += item.Count;
            }
            return total;
        }

        public int getAmountInHold(String type)
        {
            foreach (CargoItem item in _cargoItems)
            {
                if (item.Name == type)
                {
                    return item.Count;
                }
            }

            Debug.Log("-WARNING: getAmountInHold(" + type + ") failed!  Could not find in available holds!");
            return -1;
        }

        public void printHold()
        {
            foreach (CargoItem item in _cargoItems)
            {
                Debug.Log(item.Name + " : " + item.Count);
            }
            Debug.Log("Total: " + getTotalHold() + " / " + _maxHold);
        }
    }
}