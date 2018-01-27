using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShipInternals;

public class Building
{
    public readonly string Name;
    private List<CargoItem> Consume;
    private List<CargoItem> Produce;
	private List<CargoItem> Cost;
    private int spaceFreed = 0;
    private int spaceConsumed = 0;
	private int buildingCost = 0;

	private Building(string name, List<CargoItem> consume, List<CargoItem> produce, List<CargoItem> cost)
    {
        Name = name ?? "Building";
        Consume = consume ?? new List<CargoItem>();
        Produce = produce ?? new List<CargoItem>();
		Cost = cost ?? new List<CargoItem> ();
        foreach (var item in Consume)
        {
            spaceFreed += item.Volume;
        }
        foreach (var item in Produce)
        {
            spaceConsumed += item.Volume;
        }
		foreach (var item in Cost) {
			buildingCost += item.Volume;
		}
    }

    /// <summary>
    /// Produces resources by consuming resources.  Operates on a given CargoHold.  Returns true if successful.
    /// </summary>
    /// <param name="workspace">The cargohold that the building takes resources from and places results into.</param>
    public bool Tick(CargoHold workspace)
    {
        // if it doesn't have that kind of item or doesn't have enough of it...
        foreach (var item in Consume)
        {
            if (!workspace.Contains(item.Name) || workspace.GetAmountInHold(item.Name) < item.Count)
            {
                return false;
            }
        }
        // if not enough room for goods...
        if ((workspace.GetRemainingSpace() + spaceFreed) - spaceConsumed < 0)
        {
            return false;
        }

        // consume the goods to be used up
        foreach (var item in Consume)
        {
            workspace.TakeFromHold(item);
        }

        // add the goods produced
        foreach (var item in Produce)
        {
            if (!workspace.Contains(item.Name))
            {
                workspace.AddHoldType(item.Name);
            }
            workspace.AddToHold(item);
        }
        return true;
    }

    public List<CargoItem> GetConsumed()
    {
        return Consume;
    }

    public List<CargoItem> GetProduced()
    {
        return Produce;
    }
	public List<CargoItem> GetBuildingCost()
	{
		return Cost;
	}


    public delegate Building BasicEnviroment();
    public static BasicEnviroment[] BasicEnviroments = { GetEnviromentDirtFactory, GetEnviromentCometFactory, GetEnviromentRockFactory, GetEnviromentOreFactory };

    public delegate Building BasicIndustrial();
    public static BasicIndustrial[] BasicIndustry = { GetFoodFactory, GetSteelFactory, GetCopperFactory, GetTitaniumFactory, GetSiliconFactory };


    // most basic resources
    public static Building GetEnviromentDirtFactory()
    {
        return new Building("Dirt Factory",
                            new List<CargoItem>(),
                            new List<CargoItem>()
                            {
                                new CargoItem("Dirt", 1)
			}, new List<CargoItem>());
    }

    public static Building GetEnviromentCometFactory()
    {
        return new Building("Ice Mine",
                            new List<CargoItem>()
                            {
                            },
                            new List<CargoItem>()
                            {
                                new CargoItem("Water", 1)
			}, new List<CargoItem>());
    }

    public static Building GetEnviromentRockFactory()
    {
        return new Building("Rock Quarry",
                            new List<CargoItem>()
                            {
                            },
                            new List<CargoItem>()
                            {
                                new CargoItem("Rock", 1)
			}, new List<CargoItem>());
    }

    public static Building GetEnviromentOreFactory()
    {
        return new Building("Ore Mine",
                            new List<CargoItem>()
                            {
                            },
                            new List<CargoItem>()
                            {
                                new CargoItem("Dirt", 1),
                                new CargoItem("Iron Ore", 1),
                                new CargoItem("Copper Ore", 1),
                                new CargoItem("Titanium Ore", 1)
			}, new List<CargoItem>());
    }

    // level 1 resources
    public static Building GetFoodFactory()
    {
        return new Building("Farm",
                            new List<CargoItem>()
                            {
                                new CargoItem("Dirt", 1),
                                new CargoItem("Water", 1),
                            },
                            new List<CargoItem>()
                            {
                                new CargoItem("Food", 1)
			}, new List<CargoItem>());
    }

    public static Building GetSteelFactory()
    {
        return new Building("Steel Mill",
                            new List<CargoItem>()
                            {
                                new CargoItem("Iron Ore",4),
                                new CargoItem("Dirt", 1),
                            },
                            new List<CargoItem>()
                            {
                                new CargoItem("Steel", 5)
			}, new List<CargoItem>());
    }

    public static Building GetCopperFactory()
    {
        return new Building("Copper Smelter",
                            new List<CargoItem>()
                            {
                                new CargoItem("Copper Ore", 2)
                            },
                            new List<CargoItem>()
                            {
                                new CargoItem("Copper", 1)
			}, new List<CargoItem>());
    }

    public static Building GetTitaniumFactory()
    {
        return new Building("Titanium Smelter",
                            new List<CargoItem>()
                            {
                                new CargoItem("Titanium Ore", 5)
                            },
                            new List<CargoItem>()
                            {
                                new CargoItem("Titanium", 5)
			}, new List<CargoItem>());
    }

    public static Building GetSiliconFactory()
    {
        return new Building("Silicon Factory",
                            new List<CargoItem>()
                            {
                                new CargoItem("Dirt", 5),
                            },
                            new List<CargoItem>()
                            {
                                new CargoItem("Silicon", 1)
			}, new List<CargoItem>());
    }

    // level 2 resource production
    public static Building GetElectronicsFactory()
    {
        return new Building("Chip Fabricator",
                            new List<CargoItem>()
                            {
                                new CargoItem("Silicon", 1),
                                new CargoItem("Gold", 1)
                            },
                            new List<CargoItem>()
                            {
                                new CargoItem("Processor", 1)
			}, new List<CargoItem>());
    }

    public static Building GetShipPartsFactory()
    {
        return new Building("Ship Components Factory",
                            new List<CargoItem>()
                            {
                                new CargoItem("Titanium", 5),
                                new CargoItem("Processor", 1)
                            },
                            new List<CargoItem>()
                            {
                                new CargoItem("Basic Ship Components", 1)
			}, new List<CargoItem>());
    }
}