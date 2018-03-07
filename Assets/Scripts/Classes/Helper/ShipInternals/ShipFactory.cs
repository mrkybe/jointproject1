using System.Collections.Generic;
using Assets.Scripts.Classes.Helper.ShipInternals;
using Assets.Scripts.Classes.Static;

namespace Assets.Scripts.Classes.Helper.ShipInternals 
{
	public class ShipFactory:Building
	{
		private List<CargoItem> Consume;
		private List<CargoItem> Produce;
		private List<CargoItem> Cost;
		private List<CargoItem> ShipResources;
		private int spaceFreed = 0;
		private int spaceConsumed = 0;
		private int ShipCost = 0;

		public ShipFactory(string name, List<CargoItem> consume, List<CargoItem> cost): base (name, consume, null ,cost)
		{

			//Name = name ?? "ShipFactory";
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
	
		override public bool Tick(CargoHold workspace)
		{
			Planet planet = (Planet) CargoHold.Owner();
			planet.DeliveryShipCount += 1;

			// consume the goods to be used up
			foreach (var item in Consume)
			{
				workspace.TakeFromHold(item);
			}

			return true;
		}
	
	
	
	
	}
	

	
}


