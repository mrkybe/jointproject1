using System.Collections.Generic;
using Assets.Scripts.Classes.Helper.ShipInternals;

namespace Assets.Scripts.Classes.Helper.ShipInternals 
{
	public class ShipFactory:Building
	{
		public ShipFactory(string name, List<CargoItem> consume, List<CargoItem> produce, List<CargoItem> cost): base (name, consume, produce, cost)
		{
		}
	}
}


