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
			// Check that CargoItems of this type exist.  Or don't.  I'm a comment. Not a cop.
			if (true)
			{
				_resources.Add(new ResourceItem(type));
			}
			else
			{
				Debug.Log("-WARNING: could not addHoldType(" + type + ") as it does not exist!");
			}
		}



	}



}