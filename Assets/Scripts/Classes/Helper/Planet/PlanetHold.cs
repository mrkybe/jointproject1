namespace Planet
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using UnityEngine;

	public class PlanetHold
	{
		private int _maxHold;
		private List<ResourceItem>_resources;

		public PlanetHold(int maxHold_in)
		{
			_maxHold = maxHold_in;
			_resources = new List<ResourceItem>();
		}

		public void addHoldType(String type)
		{
			// Check that ResourceItems of this type exist. 
			if (true)
			{
				_resources.Add(new ResourceItem(type));
			}
			else
			{
				Debug.Log("-WARNING: could not addHoldType(" + type + ") as it does not exist!");
			}
		}

		public void addToHold(String type, int count)
		{
			// Check that ResrouceItem of this type exist.
			if (this.Contains(type))
			{
				for (int i = 0; i < _resources.Count; i++)
				{
					if (_resources[i].Name == type)
					{
						Debug.Log("In Hold!: " + _resources[i].Count + " adding " + count);
						_resources[i].Count = count;
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
			for (int i = 0; i < _resources.Count; i++)
			{
				if (_resources[i].Name == type)
				{
					contains = true;
				}
			}
			return contains;
		}

		public int getTotalHold()
		{
			int total = 0;
			foreach (ResourceItem item in _resources)
			{
				total += item.Count;
			}
			return total;
		}

		public int getAmountInHold(String type)
		{
			foreach (ResourceItem item in _resources)
			{
				if (item.Name == type)
				{
					return item.Count;
				}
			}

			Debug.Log("-WARNING: getAmountInHold(" + type + ") failed!  Could not find in available holds!");
			return -1;
		}

	}



}