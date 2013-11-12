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
    public static class CargoItemTypes
    {
        private static List<CargoItem> availableItemTypes;

        static CargoItemTypes()
        {
            // private constructor ensures no instances of this class are created
            availableItemTypes = new List<CargoItem>();
        }

        public CargoItem GetItemOfType(string type)
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

    public class CargoHold
    {
        int _maxHold;
        List<CargoBay> _cargoBays;

        // CargoHolds should always be declared with the maximum amount of space they contain in units.
        public CargoHold(int maxHold_in)
        {
            _maxHold = maxHold_in;
        }

        public void addHoldType(CargoItem type)
        {
            // Check that CargoItems of this type exist.
            if (CargoItemTypes.Contains(type))
            {
                _cargoBays.Add(new CargoBay(type));
            }
            else
            {
                Debug.Log("-WARNING: could not addHoldType(" + type.Name + ") as it does not exist!");
            }
        }

        public void addHoldType(String type)
        {
            // Check that CargoItems of this type exist.
            Debug.Log("addHoldType(String type): Trying to add: " + type);
            if (CargoItemTypes.Contains(type))
            {
                Debug.Log("addHoldType(String type): Yes '" + type + "' exists as a CargoItemType;");
                Debug.Log(CargoItemTypes.GetItemOfType("Fish").Name);
                _cargoBays.Add(new CargoBay(CargoItemTypes.GetItemOfType(type)));
            }
            else
            {
                Debug.Log("-WARNING: could not addHoldType(" + type + ") as it does not exist!");
            }
        }

        public void printHold()
        {
            foreach (CargoBay bay in _cargoBays)
            {
                Debug.Log(bay.TypeName + " : " + bay.Amount);
            }
        }
        
    }

    public class CargoBay
    {
        int _amount;
        CargoItem _type;

        public CargoBay(CargoItem type_in)
        {
            Debug.Log("CREATED A CARGO BAY OF " + type_in.Name);
            _type = type_in;
        }

        public string TypeName
        {
            get { return _type.Name; }
        }

        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
    }

    public class CargoItem
    {
        string _name; // use something better than a string for this, data driven ideally, ie: xml file somewhere that has all of the resource definitions and is parsed into cargo item types.
        int _size;
        int baseValue;

        public CargoItem(string name_in)
        {
            _name = name_in;
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
