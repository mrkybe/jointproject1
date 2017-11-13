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
    
    [Serializable]
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

        private CargoHold(List<CargoItem> items)
        {
            _cargoItems = items;
            _maxHold = GetTotalHold();
        }

        public void AddHoldType(String type)
        {
            // would be nice to have an authoriative list of cargoitems that exist
            if (!Contains(type))
            {
                _cargoItems.Add(new CargoItem(type));
            }
            else
            {
                Debug.Log("-WARNING: could not AddHoldType(" + type + ") as it already exists on this cargohold!");
            }
        }

        public void AddToHold(CargoItem item)
        {
            AddToHold(item.Name, item.Count);
        }

        public void TakeFromHold(CargoItem item)
        {
            AddToHold(item.Name, -item.Count);
        }

        public void AddToHold(String type, int count)
        {
            // Check that CargoItems of this type exist.
            if (this.Contains(type))
            {
                for (int i = 0; i < _cargoItems.Count; i++)
                {
                    if (_cargoItems[i].Name == type)
                    {
                        //Debug.Log("In Hold!: " + _cargoItems[i].Count + " adding " + count);
                        _cargoItems[i].Count = count;
                    }
                }
            }
            else
            {
                Debug.Log("-WARNING: could not AddToHold(" + type + ") as it does not exist in this instace!");
            }
        }

		public List<string> getCargoItems()
		{
			var availableStocks = new List<string> ();

			foreach(var item in _cargoItems)
			{
				//string returnName = s;
				item.Name
				availableStocks.Add(item);
			}
			return availableStocks;

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

        public int GetTotalHold()
        {
            int total = 0;
            foreach (CargoItem item in _cargoItems)
            {
                total += item.Count;
            }
            return total;
        }

        public int GetRemainingSpace()
        {
            return _maxHold - GetTotalHold();
        }

        public int GetAmountInHold(String type)
        {
            foreach (CargoItem item in _cargoItems)
            {
                if (item.Name == type)
                {
                    return item.Count;
                }
            }

            Debug.Log("-WARNING: GetAmountInHold(" + type + ") failed!  Could not find in available holds!");
            return -1;
        }

        public override string ToString()
        {
            string result = "";
            foreach (CargoItem item in _cargoItems)
            {
                result += item.Name + " : " + item.Count + "\n";
            }
            result += "Total: " + GetTotalHold() + " / " + _maxHold;
            return result;
        }

        public void PrintHold()
        {
            Debug.Log(ToString());
        }

        public int Credit(String type, CargoHold source, int amount)
        {
            if (this.Contains(type) && source.Contains(type))
            {
                int maxTransferable = Mathf.Min(GetRemainingSpace(), amount, source.GetAmountInHold(type));
                AddToHold(type, maxTransferable);
                source.AddToHold(type, -maxTransferable);
                return maxTransferable;
            }
            return 0;
        }

        // 
        public static CargoHold GetAsteroidFieldCargoHold()
        {
            List<CargoItem> items = new List<CargoItem>();
            int hashcode = UnityEngine.Random.value.GetHashCode();
            System.Random random = new System.Random(hashcode);

            RandomAdder("Dirt"        , random.Next(250) + random.Next(250) - 150, items);
            RandomAdder("Water"       , random.Next(250) + random.Next(250) - 150, items);
            RandomAdder("Rock"        , random.Next(250) + random.Next(250) - 150, items);
            RandomAdder("Iron Ore"    , random.Next(250) - 100                   , items);
            RandomAdder("Copper Ore"  , random.Next(250) - 150                   , items);
            RandomAdder("Titanium Ore", random.Next(250) - 200                   , items);
            RandomAdder("Gold"        , random.Next(100) - 75                    , items);

            return new CargoHold(items);
        }

        private static void RandomAdder(string itemName, int add, List<CargoItem> items)
        {
            if (add > 0)
            {
                items.Add(new CargoItem(itemName, add));
            }
        }

        private static int Clamp01(int value)
        {
            return Mathf.Clamp(value, 0, int.MaxValue);
        }
    }
}