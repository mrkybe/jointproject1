using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EntityParts
{
    /* Written by Ruslan Kaybyshev, Late 2013
     * Purpose: One centralized place where data structure type classes can be
     *          stored easily.  These data structure types (Entity Components)
     *          can be used to easily add common functionality to any
     *          entities that are implimented later and need shared data types.
     */

    /* How SensorArray should work in theory:
     * 1.  I have a class that needs to be intimately aware of its surroundings
     *     and own abilities.
     * 2.  I attach a SensorArray to that class
     * 3.  I set up Sensors on the array.
     * 4.  It populates data that it can determine based on the sensor
     *     a. Example Sensor:
     *        i. near by objects
     *           1. can search for specific type &&|| range &&|| size
     *              &&|| faction &&|| threat_level &&|| health
     */

    public class SensorArray
    {
        GameObject _owner;
        Spaceship myShipScript;
        
        public SensorArray(GameObject owner_in)
        {
            _owner = owner_in;
            myShipScript = _owner.transform.GetComponent<Spaceship>();
        }

        public float StoppingDistance
        {
            //d = (vf^2 +  vi^2) / 2a
            get
            {
                //Debug.Log("GETTING STOPPING DISTANCE~");
                float distance = ((myShipScript.EngineRunSpeed * myShipScript.EngineRunSpeed) / (2 * myShipScript.EngineAcceleration)) * Time.fixedDeltaTime;
                Debug.Log("STOPPING DISTANCE IS: " + distance);
                //Debug.Log("STOPPING TIME IS: " + stoppingTime);
                return distance;
            }
        }

        public float EngineRunSpeed
        {
            get { return myShipScript.EngineRunSpeed; }
        }
        public float EngineAcceleration
        {
            get { return myShipScript.EngineAcceleration; }
        }
        public float MaxSpeed
        {
            get { return myShipScript.MaxSpeed; }
        }
        public float TurningSpeed
        {
            get { return myShipScript.TurningSpeed; }
        }

        public float Manuverability
        {
            get { return myShipScript.Manuverability; }
        }

        public float TargetSpeed
        {
            get { return myShipScript.TargetSpeed; }
        }
    }

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

        public void addHoldType(CargoItem type)
        {
            // Check that CargoItems of this type exist.
            if (CargoItemTypes.Contains(type))
            {
                _cargoItems.Add(CargoItemTypes.GetItemOfType(type.Name));
            }
            else
            {
                Debug.Log("-WARNING: could not addHoldType(" + type.Name + ") as it does not exist!");
            }
        }

        public void addHoldType(String type)
        {
            // Check that CargoItems of this type exist.
            if (CargoItemTypes.Contains(type))
            {
                _cargoItems.Add(CargoItemTypes.GetItemOfType(type));
            }
            else
            {
                Debug.Log("-WARNING: could not addHoldType(" + type + ") as it does not exist!");
            }
        }

        public void addToHold(String type, int count)
        {
            // Check that CargoItems of this type exist.
            if (CargoItemTypes.Contains(type) && this.Contains(type))
            {
                for (int i = 0; i < _cargoItems.Count; i++)
                {
                    if (_cargoItems[i].Name == type)
                    {
                        _cargoItems[i].Count = _cargoItems[i].Count + count;
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

    public class CargoItem
    {
        private string _name;
        private int _size;
        private int _baseValue;
        private int _count;

        public CargoItem(string name_in)
        {
            _name = name_in;
            _baseValue = 1;
            _count = 0;
            _size = 1;
        }

        public CargoItem(string name_in, int baseValue_in)
        {
            _name = name_in;
            _baseValue = baseValue_in;
            _count = 0;
            _size = 1;
        }

        public CargoItem(string name_in, int baseValue_in, int size_in)
        {
            _name = name_in;
            _baseValue = baseValue_in;
            _size = size_in;
            _count = 0;
        }

        public CargoItem(string name_in, int baseValue_in, int size_in, int count_in)
        {
            _name = name_in;
            _baseValue = baseValue_in;
            _count = count_in;
            _size = size_in;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Count
        {
            get { return _count; }
            set { _count += value; }
        }
    }

    public static class CargoItemTypes
    {
        private static List<CargoItem> availableItemTypes;

        static CargoItemTypes()
        {
            // private constructor ensures no instances of this class are created
            availableItemTypes = new List<CargoItem>();
        }

        public static CargoItem GetItemOfType(string type)
        {
            CargoItem wantedCargo = null;
            for (int i = 0; i < availableItemTypes.Count; i++)
            {
                if (availableItemTypes[i].Name == type)
                {
                    wantedCargo = availableItemTypes[i];
                }
            }
            if (wantedCargo == null)
            {
                Debug.Log("WARNING - GetItemOfType() returned null!  Item of type '" + type + "' not found!");
            }
            return wantedCargo;
        }

        public static bool Contains(CargoItem type)
        {
            bool contains = false;
            for (int i = 0; i < availableItemTypes.Count; i++)
            {
                if (availableItemTypes[i].Name == type.Name)
                {
                    contains = true;
                }
            }
            return contains;
        }

        public static bool Contains(String type)
        {
            bool contains = false;
            for (int i = 0; i < availableItemTypes.Count; i++)
            {
                if (availableItemTypes[i].Name == type)
                {
                    contains = true;
                }
            }
            return contains;
        }

        public static bool AddCargoItemType(CargoItem type)
        {
            if (!Contains(type))
            {
                availableItemTypes.Add(type);
                Debug.Log("-NOTE: Created CargoItemType(" + type.Name + ")!");
                return true;
            }
            else
            {
                Debug.Log("-WARNING: could not addHoldType(" + type.Name + ") as it already exists!");
                return false;
            }
        }
    }
}
