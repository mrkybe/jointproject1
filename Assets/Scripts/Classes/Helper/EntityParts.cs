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

        public void printHold()
        {
            foreach (CargoItem item in _cargoItems)
            {
                Debug.Log(item.Name + " : " + item.Count);
            }
        }
        
    }

    public class CargoItem
    {
        private string _name; // use something better than a string for this, data driven ideally, ie: xml file somewhere that has all of the resource definitions and is parsed into cargo item types.
        private int _size;
        private int baseValue;
        private int count;

        public CargoItem(string name_in)
        {
            _name = name_in;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Count
        {
            get { return count; }
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
                    Debug.Log(wantedCargo.Name);
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
