using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.WorldSingleton;
using ShaderForge;
using UnityEngine;

namespace Assets.Scripts.Classes.Helper.ShipInternals
{
    /* How Cargo should work in theory:
         * 1.  I have a class that needs to store Cargo
         * 2.  I attach a CargoHold to that class
         * 3.  The CargoHold is basically a collection of CargoItem that adds useful methods
         * 4.  CargoItem is basically a single cargo bay that tracks the amount of a given CargoItem, its name, etc.
         */
    
    [Serializable]
    public class CargoHold
    {
        private bool _real; 
        private int _maxHold;
        private MonoBehaviour _owner;
        private List<CargoItem> _cargoItems;
        private SerializableDictionary<string, float> supplyDemandCostModifier;

        // CargoHolds should always be declared with the maximum amount of space they contain in units.
        public CargoHold(MonoBehaviour owner, int maxHold_in)
        {
            _maxHold = maxHold_in;
            _cargoItems = new List<CargoItem>();
            _owner = owner;
            supplyDemandCostModifier = new SerializableDictionary<string, float>();
        }

        private CargoHold(MonoBehaviour owner, List<CargoItem> items)
        {
            _cargoItems = items;
            _maxHold = GetTotalHold();
            _owner = owner;
            supplyDemandCostModifier = new SerializableDictionary<string, float>();
        }

        public IEnumerable<CargoItem> CargoItems
        {
            get
            {
                List<CargoItem> result = new List<CargoItem>();
                foreach (CargoItem r in _cargoItems)
                {
                    result.Add(r.Copy());
                }
                return result;
            }
        }

        public void SetCostModifier(string ResourceName, float mulitplier)
        {
            supplyDemandCostModifier[ResourceName] = mulitplier;
        }

        public void AddHoldType(String type)
        {
            // would be nice to have an authoriative list of cargoitems that exist
            if (!Contains(type))
            {
                _cargoItems.Add(new CargoItem(type));
                if (!supplyDemandCostModifier.ContainsKey(type))
                {
                    supplyDemandCostModifier.Add(type, 1.0f);
                }
            }
            else
            {
                //Debug.Log("-WARNING: could not AddHoldType(" + type + ") as it already exists on this cargohold!");
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
                        _cargoItems[i].Count += count;
                    }
                }
            }
            else
            {
                Debug.Log("-WARNING: could not AddToHold(" + type + ") as it does not exist in this instance!");
            }
        }

		public List<string> GetCargoItems()
		{
			var availableStocks = new List<string> ();

			foreach(var item in _cargoItems)
			{
				availableStocks.Add(item.Name);
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

            //Debug.Log("-WARNING: GetAmountInHold(" + type + ") failed!  Could not find in available holds!");
            return 0;
        }

        public override string ToString()
        {
            string result = "";
            if (_cargoItems == null)
            {
                return "No CargoItem List Initialized Yet";
            }
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

        /// <summary>
        /// Moves 'amount' of a resource into this cargohold from another.  Returns the amount of the resource transfered.
        /// </summary>
        /// <param name="type">Resource name</param>
        /// <param name="source">Cargohold to take from</param>
        /// <param name="amount"></param>
        /// <param name="AutoCreateHold">Automatically add the type of hold necessary to store the resource?</param>
        /// <returns></returns>
        public int Credit(String type, CargoHold source, int amount, bool AutoCreateHold = false)
        {
            if (AutoCreateHold)
            {
                if (!this.Contains(type))
                {
                    AddHoldType(type);
                }
            }
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
        public static CargoHold GenerateAsteroidFieldCargoHold(AsteroidField field)
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

            return new CargoHold(field, items);
        }

        public MonoBehaviour Owner()
        {
            return _owner;
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

        public int GetMoneyValue()
        {
            return _cargoItems.Sum(x => x.Cost);
        }

        public int GetCargoItemValue(string name)
        {
            if (Contains(name))
                return (int) (supplyDemandCostModifier[name] * _cargoItems.First(x => x.Name == name).Cost);
            else
                return 0;
        }

        public int GetCargoItemUnitCost(string name)
        {
            if (Contains(name))
                return (int)(supplyDemandCostModifier[name] * _cargoItems.First(x => x.Name == name).UnitCost);
            else
                return CargoItem.resources_in_use.First(x => x.Name == name).BaseValue;
        }
    }
}