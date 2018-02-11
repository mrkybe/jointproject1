﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Classes.Helper.Pilot;
using Assets.Scripts.Classes.Helper.ShipInternals;
using Assets.Scripts.Classes.Mobile;
using Assets.Scripts.Classes.WorldSingleton;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Classes.Static {
    /// <summary>
    /// The AI portion of the Planet class.
    /// </summary>
    public partial class Planet: Static
    {
        private List<CargoItem> consumableCargoItems;
        private List<CargoItem> producableCargoItems;
        private List<CargoItem> itemsNetChange;
        private List<MarketOrder> deliveryList;
        private List<MarketOrder> deliveryInProgressList;
        private List<MarketOrder> deliveryFailedList;
        private List<GameObject> ReadyDeliveryShips;
        private GameObject DeliveryShip;
        public int DeliveryShipCount = 0;
        private float LastDeliveryShipDeployment = 0;

        void PlanetBTSetup()
        {
            //behaviorTree = CreatePlanetBT();

            LastDeliveryShipDeployment = Time.time;

            //blackboard = behaviorTree.Blackboard;

            // attach the debugger component if executed in editor (helps to debug in the inspector) 
            DeliveryShipCount = (Random.value > 0.5f ? 1 : 0);
            consumableCargoItems = new List<CargoItem>();
            producableCargoItems = new List<CargoItem>();
            itemsNetChange = new List<CargoItem>();
            deliveryList = new List<MarketOrder>();
            deliveryInProgressList = new List<MarketOrder>();
            deliveryFailedList = new List<MarketOrder>();
            ReadyDeliveryShips = new List<GameObject>();
            DeliveryShip = (GameObject)Resources.Load("Prefabs/AI_ship");
            InvokeRepeating("UpdateEverything", 1, 0.75f + (Random.value/2f));
        
            //behaviorTree.Start();
        }

        private void UpdateEverything()
        {
            CalculateConsumableResources();
            CalculateUnwantedResource();
            UpdateMarketSellingBuyingOrders();
            SendDeliveryShips();
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

        /// <summary>
        /// Returns the net gain/loss of resources as a result of the Planet's Buildings operating one tick.
        /// </summary>
        /// <returns></returns>
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
                    reservedStorage.Credit(item, myStorage, itemSupply, true);
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
                if (!orders.ContainsKey(order.destination))
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
                    if (DeliveryShipCount > 0 || ReadyDeliveryShips.Count > 0)
                    {
                        if ( order.item.Count > 0)
                        {
                            SendDeliveryShip(order);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Allows the Unity Editor to call this method.  Don't use this unless you know what you're doing.
        /// </summary>
        /// <param name="typename"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public Spaceship SpawnSpaceship(string typename, int number)
        {
            Vector2 offset = Random.insideUnitCircle.normalized * (this.transform.localScale.magnitude + 1);
            Vector3 offset3d = new Vector3(offset.x, 0, offset.y);

            Quaternion shipRotation = Quaternion.LookRotation(offset3d, Vector3.up);
            GameObject ship = null;

            ship = Instantiate(DeliveryShip, this.transform.position + offset3d, shipRotation);
            AI_Patrol pilot = ship.GetComponent<AI_Patrol>();
            Spaceship shipScript = ship.GetComponent<Spaceship>();
            ship.name = "S_" + typename + "_" + this.Faction.Name + "_" + this.MyName + "_" + number;
            shipScript.Faction = Faction;

            return shipScript;
        }

        /// <summary>
        /// Allows the Unity Editor to call this method.  Don't use this unless you know what you're doing.
        /// </summary>
        /// <param name="miningTargetList"></param>
        /// <returns></returns>
        public Spaceship SpawnMiningShip(List<string> miningTargetList)
        {
            Spaceship shipScript = SpawnSpaceship("Miner", DeliveryShipCount + WorkerShips.Count);
            AI_Patrol pilot = (AI_Patrol)shipScript.GetPilot;
        
            CargoHold shipHold = shipScript.GetCargoHold;
            foreach(string resource in miningTargetList)
                shipHold.AddHoldType(resource);
        
            pilot.StartMining(miningTargetList, this);
        

            WorkerShips.Add(shipScript.gameObject);
            return shipScript;
        }

        private void SendDeliveryShip(MarketOrder order)
        {
            Spaceship shipScript = null;
            if (ReadyDeliveryShips.Count == 0)
            {
                shipScript = SpawnSpaceship("Transport", DeliveryShipCount);
                DeliveryShipCount--;
            }
            else
            {
                shipScript = ReadyDeliveryShips[0].GetComponent<Spaceship>();
                ReadyDeliveryShips.RemoveAt(0);
            }
            GameObject ship = shipScript.gameObject;
        
            CargoHold shipHold = shipScript.GetCargoHold;
            shipHold.AddHoldType(order.item.Name);
            int transfered = shipHold.Credit(order.item.Name, reservedStorage, order.item.Count);
            order.item.Count -= transfered;
            if (order.item.Count == 0)
            {
                deliveryList.Remove(order);
                deliveryInProgressList.Add(order);
            }

            AI_Patrol pilot = ship.GetComponent<AI_Patrol>();
            if (pilot != null)
            {
                pilot.StartDelivery(order);
            }
            WorkerShips.Add(ship.gameObject);
        }

        /// <summary>
        /// Registers a Delivery Ship as having docked to the Planet.
        /// </summary>
        /// <param name="aiPatrol"></param>
        public void ReturnDeliveryShip(AI_Patrol aiPatrol)
        {
            ReadyDeliveryShips.Remove(aiPatrol.gameObject);
            DeliveryShipCount++;
        }

        /// <summary>
        /// Adds an MarketOrder to be fulfilled by the Planet.
        /// </summary>
        /// <param name="marketOrder"></param>
        public void AddToDeliveryQueue(MarketOrder marketOrder)
        {
            deliveryList.Add(marketOrder);
        }

        /// <summary>
        /// Registers a Delivery Ship as having parked in orbit around the Planet.
        /// </summary>
        /// <param name="aiPatrol"></param>
        public void AddToAvailableDeliveryShips(AI_Patrol aiPatrol)
        {
            ReadyDeliveryShips.Add(aiPatrol.gameObject);
        }


        /// <summary>
        /// Registers a MarketOrder as completed.
        /// </summary>
        /// <param name="marketOrder"></param>
        public void CompleteOrder(MarketOrder marketOrder)
        {
            if (deliveryInProgressList.Contains(marketOrder))
            {
                deliveryInProgressList.Remove(marketOrder);
            }
            else
            {
                Debug.Log("TRIED TO COMPLETE ORDER THAT ISN'T IN PROGRESS - BIG PROBLEM");
            }
        }

        /// <summary>
        /// Registers a MarketOrder as failed to complete.  ie: Pirates killed the delivery ship.
        /// </summary>
        /// <param name="marketOrder"></param>
        public void FailOrder(MarketOrder marketOrder)
        {
            if (deliveryInProgressList.Contains(marketOrder))
            {
                deliveryInProgressList.Remove(marketOrder);
                deliveryFailedList.Add(marketOrder);
            }
            else
            {
                Debug.Log("TRIED TO FAIL ORDER THAT ISN'T IN PROGRESS - BIG PROBLEM");
            }
        }
    }
}


