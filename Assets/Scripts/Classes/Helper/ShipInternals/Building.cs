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
    private int spaceFreed = 0;
    private int spaceConsumed = 0;

    public Building(string name, List<CargoItem> consume, List<CargoItem> produce)
    {
        Name = name ?? "Building";
        Consume = consume ?? new List<CargoItem>();
        Produce = produce ?? new List<CargoItem>();
        foreach (var item in Consume)
        {
            spaceFreed += item.Volume;
        }
        foreach (var item in Produce)
        {
            spaceConsumed += item.Volume;
        }
    }

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

    // most basic resources
    public static Building GetEnviromentDirtFactory()
    {
        return new Building("Dirt Factory",
                            new List<CargoItem>(),
                            new List<CargoItem>()
                            {
                                new CargoItem("Dirt", 10)
                            });
    }

    public static Building GetEnviromentCometFactory()
    {
        return new Building("Ice Mine",
                            new List<CargoItem>()
                            {
                            },
                            new List<CargoItem>()
                            {
                                new CargoItem("Water", 10)
                            });
    }

    public static Building GetEnviromentRockFactory()
    {
        return new Building("Ice Mine",
                            new List<CargoItem>()
                            {
                            },
                            new List<CargoItem>()
                            {
                                new CargoItem("Rock", 10)
                            });
    }

    public static Building GetEnviromentOreFactory()
    {
        return new Building("Ore Mine",
                            new List<CargoItem>()
                            {
                            },
                            new List<CargoItem>()
                            {
                                new CargoItem("Dirt", 5),
                                new CargoItem("Iron Ore", 3),
                                new CargoItem("Copper Ore", 2),
                                new CargoItem("Titanium Ore", 1)
                            });
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
                            });
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
                            });
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
                            });
    }

    public static Building GetTitaniumFactory()
    {
        return new Building("Titanium Smelter",
                            new List<CargoItem>()
                            {
                                new CargoItem("Titanium Ore",5)
                            },
                            new List<CargoItem>()
                            {
                                new CargoItem("Titanium", 1)
                            });
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
                            });
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
                            });
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
                            });
    }
}