using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.WorldSingleton;
using ShipInternals;
using NPBehave;
using Action = NPBehave.Action;

public partial class Planet: Static
{
	protected Root behaviorTree;
	private Blackboard blackboard;

    private List<CargoItem> consumableCargoItems;
    private List<CargoItem> producableCargoItems;
    private List<CargoItem> itemsNetChange;
    private List<MarketOrder> deliveryList;

    void PlanetBTSetup()
	{
        behaviorTree = CreatePlanetBT();

		blackboard = behaviorTree.Blackboard;

		// attach the debugger component if executed in editor (helps to debug in the inspector) 
		#if UNITY_EDITOR
		Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
		debugger.BehaviorTree = behaviorTree;
        #endif

	    consumableCargoItems = new List<CargoItem>();
	    producableCargoItems = new List<CargoItem>();
        itemsNetChange = new List<CargoItem>();
	    deliveryList = new List<MarketOrder>();

        behaviorTree.Start();
	}

	private Root CreatePlanetBT()
	{
		return new Root (
			new Sequence (
				new Wait (10f),
				new Action (() => {
					CalculateConsumableResources ();
				}){ Label = "Calculate the resources needed for the market" },
				new Action (() => {
					CalculateUnwantedResource ();
				}){ Label = "Calculate the resources that doesn't need" },
				new Action (() => {
					UpdateMarketSellingBuyingOrders();
				}){ Label = "Update the market selling orders and buying orders" },
				new Action (() => {
					SendDeliveryShips ();
				}){ Label = "Send delivery ships for sold resources if ships are available" }
			

			)
		);
	}

    private void CalculateConsumableResources()
    {
        consumableCargoItems.Clear();
        foreach (Building b in myBuildings)
        {
            consumableCargoItems.AddRange(b.GetConsumed());
        }
    }

    private void CalculateProducableResources()
    {
        producableCargoItems.Clear();
        foreach (Building b in myBuildings)
        {
            producableCargoItems.AddRange(b.GetProduced());
        }
    }

    public CargoHold CalculateNetDemand()
    {
        CargoHold temp = new CargoHold(Int32.MaxValue);
        CalculateProducableResources();
        CalculateConsumableResources();
        foreach (CargoItem item in producableCargoItems)
        {
            temp.AddHoldType(item.Name);
        }
        foreach (CargoItem item in consumableCargoItems)
        {
            temp.AddHoldType(item.Name);
        }
        // 
        foreach (CargoItem item in producableCargoItems)
        {
            temp.AddToHold(item);
        }
        foreach (CargoItem item in consumableCargoItems)
        {
            temp.TakeFromHold(item);
        }
        return temp;
    }

	private void CalculateUnwantedResource()
	{
	}

	private void UpdateMarketSellingBuyingOrders()
	{
	    Overseer market = Overseer.Main;
	    CargoHold netDemand = CalculateNetDemand();
	    foreach (string item in netDemand.GetCargoItems())
	    {
	        int amountPossessed = myStorage.GetAmountInHold(item);
	        int itemSupply = netDemand.GetAmountInHold(item);
	        int itemDemand = -itemSupply;
            if (itemSupply > 0 && amountPossessed > itemSupply)
            {
                CargoItem transaction = new CargoItem(item, itemSupply);
                MarketOrder order = new MarketOrder(this, transaction);
                reservedStorage.Credit(item, myStorage, itemSupply);
                market.PlaceSellOrder(order);
            }
	        if (itemDemand > 0 && true) // check that we can afford it..?  somehow?
	        {
	            CargoItem transaction = new CargoItem(item, itemDemand);
	            MarketOrder order = new MarketOrder(this, transaction);
	            market.PlaceBuyOrder(order);
            }
        }
	}

	private void SendDeliveryShips()
	{
	    Dictionary<Planet, List<MarketOrder>> orders = new Dictionary<Planet, List<MarketOrder>>();
	    foreach (MarketOrder order in deliveryList)
	    {
	        if (orders[order.destination] == null)
	        {
	            orders[order.destination] = new List<MarketOrder>();
	        }
	        orders[order.destination].Add(order);
	    }

	    foreach (Planet p in orders.Keys)
	    {
	        var list = orders[p];
	        foreach (var order in list)
	        {
	            SendDeliveryShip(order);
	        }
	    }
	}

    private void SendDeliveryShip(MarketOrder order)
    {
        var ship = (GameObject)Instantiate(Resources.Load("Prefabs/AI_ship"), this.transform.position, Quaternion.identity);
        AI_Patrol pilot = ship.GetComponent<AI_Patrol>();

        WorkerShips.Add(ship.gameObject);
    }

    public void AddToDeliveryQueue(MarketOrder marketOrder)
    {
        deliveryList.Add(marketOrder);
    }
}


