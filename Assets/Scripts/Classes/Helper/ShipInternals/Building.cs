using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShipInternals;

public class Building
{
	private List<CargoItem> Consume;
    private List<CargoItem> Produce;
    private int spaceFreed = 0;
    private int spaceConsumed = 0;

    public Building(List<CargoItem> consume, List<CargoItem> produce)
    {
        Consume = consume;
        Produce = produce;
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
}