using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Classes.Helper.ShipInternals
{
    [Serializable]
    public class Resource
    {
        private string _name;
        private int _size;
        private float _baseValue;

        public Resource(string name, int size, float baseValue)
        {
            _name = name;
            _size = size;
            _baseValue = baseValue;
            CargoItem.resources_in_use.Add(this);
        }

        public string Name
        {
            get { return _name; }
        }

        public int Size
        {
            get { return _size; }
        }

        public float BaseValue
        {
            get { return _baseValue; }
        }
    }

    /// <summary>
    /// A CargoItem keeps track of a single kind of resource in a CargoHold.
    /// </summary>

    [Serializable]
    public class CargoItem
    {
        public static List<Resource> resources_in_use = new List<Resource>();
        private Resource myResource = null;
        private int _count;

        private CargoItem()
        {
        }

        public CargoItem(string name_in)
        {
            FullConstructor(name_in);
        }

        public CargoItem(string name_in, int count_in)
        {
            FullConstructor(name_in, count_in);
        }

        public CargoItem(string name_in, int count_in, int size_in)
        {
            FullConstructor(name_in, count_in, size_in);
        }

        public CargoItem(string name_in, int count_in, int size_in, float base_value)
        {
            FullConstructor(name_in, count_in, size_in, base_value);
        }

        private void FullConstructor(string name_in, int count_in = 0, int size_in = 1, float base_value = 1)
        {
            if (resources_in_use.Count != 0)
            {
                myResource = resources_in_use.FirstOrDefault(x => x.Name == name_in);
            }
            if (myResource == null)
            {
                myResource = new Resource(name_in, size_in, base_value);
            }
            _count = count_in;
        }

        public string Name
        {
            get { return myResource.Name; }
        }

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public int Size
        {
            get { return myResource.Size; }
        }

        public int Volume
        {
            get { return Size * Count; }
        }

        public float Cost
        {
            get { return Count * myResource.BaseValue; }
        }

        /// <summary>
        /// Are these the same kind of CargoItem?
        /// </summary>
        /// <param name="buyOrderItem"></param>
        /// <returns></returns>
        public bool KindEquals(CargoItem buyOrderItem)
        {
            return this.Name.Equals(buyOrderItem.Name);
        }

        internal CargoItem Copy()
        {
            return new CargoItem(myResource.Name, _count);
        }
    }
}