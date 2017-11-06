namespace ShipInternals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    [Serializable]
    public class CargoItem
    {
        public static List<String> types_in_use = new List<string>();
        private string _name;
        private int _size;
        private int _baseValue;
        private int _count;

        public CargoItem(string name_in)
        {
            _name = name_in;
            _size = 1;
            _count = 0;
            types_in_use.Add(name_in);
        }

        public CargoItem(string name_in, int count_in)
        {
            _name = name_in;
            _count = count_in;
            _size = 1;
            types_in_use.Add(name_in);
        }

        public CargoItem(string name_in, int count_in, int size_in)
        {
            _name = name_in;
            _size = size_in;
            _count = count_in;
            types_in_use.Add(name_in);
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

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public int Volume
        {
            get { return Size * Count; }
        }
    }
}