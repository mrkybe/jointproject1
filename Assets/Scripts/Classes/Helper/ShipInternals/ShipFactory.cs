using System.Collections.Generic;
using Assets.Scripts.Classes.Helper.ShipInternals;

namespace Assets.Scripts.Classes.Helper.ShipInternals 
{
	public class ShipFactory:Building
	{
		public readonly string Name;
		private List<CargoItem> Consume;
		private List<CargoItem> Produce;
		private List<CargoItem> Cost;
		private List<CargoItem> ShipResources;
		private int spaceFreed = 0;
		private int spaceConsumed = 0;
		private int ShipCost = 0;

		public ShipFactory(string name, List<CargoItem> consume, List<CargoItem> cost): base (name, consume, null ,cost)
		{

			Name = name ?? "ShipFactory";
			Consume = consume ?? new List<CargoItem>();
			Cost = cost ?? new List<CargoItem> ();


			foreach (var item in Consume)
			{
				spaceFreed += item.Volume;
			}
			foreach (var item in Cost) 
			{
				ShipCost += item.Volume;
			}
		}
		public delegate ShipFactory ShipResource();
		//public static ShipResource[] ShipElements = { base.GetEnviromentDirtFactory, base.GetEnviromentCometFactory, base.GetEnviromentRockFactory, base.GetEnviromentOreFactory };
	
	
	
	
	
	
	}
	

	
}


