using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Classes.Helper.ShipInternals;
using Assets.Scripts.Classes.Static;
using UnityEngine;

namespace Assets.Scripts.Classes.WorldSingleton
{
    public partial class Overseer : Static.Static
    {
        // This part of the singleton keeps track of buying and selling resources between planets along with matching orders.

        public Dictionary<Planet, List<MarketOrder>> sellingOrdersByPlanet;
        public Dictionary<Planet, List<MarketOrder>> buyingOrdersByPlanet;
        public Dictionary<Planet, MarketOrder> executedOrders;

        private void CreateMarket()
        {
            //sellingOrders = new List<MarketOrder>();
            //buyingOrders = new List<MarketOrder>();

            sellingOrdersByPlanet = new Dictionary<Planet, List<MarketOrder>>();
            buyingOrdersByPlanet = new Dictionary<Planet, List<MarketOrder>>();
            executedOrders = new Dictionary<Planet, MarketOrder>();

            foreach (Planet p in Planet.listOfPlanetObjects)
            {
                sellingOrdersByPlanet.Add(p, new List<MarketOrder>());
                buyingOrdersByPlanet.Add(p, new List<MarketOrder>());
            }
        }

        /// <summary>
        /// Adds a MarketOrder containing resources for sale.
        /// </summary>
        /// <param name="order"></param>
        public void PlaceSellOrder(MarketOrder order)
        {
            foreach (MarketOrder existing in sellingOrdersByPlanet[order.origin])
            {
                if (existing.item.KindEquals(order.item))
                {
                    existing.Combine(order);
                    return;
                }
            }

            sellingOrdersByPlanet[order.origin].Add(order);
        }

        /// <summary>
        /// Adds a MarketOrder containing resources to buy.
        /// </summary>
        /// <param name="order"></param>
        public void PlaceBuyOrder(MarketOrder order)
        {
            foreach (MarketOrder existing in buyingOrdersByPlanet[order.origin])
            {
                if (existing.item.KindEquals(order.item))
                {
                    existing.Combine(order);
                    return;
                }
            }

            buyingOrdersByPlanet[order.origin].Add(order);
        }


        private void CreateResourceTypes()
        {
            List<Resource> resources = new List<Resource>()
            {
                new Resource("Food", 1, 100),

                new Resource("Dirt", 1, 10),
                new Resource("Rock", 1, 10),
                new Resource("Water", 1, 20),

                new Resource("Iron Ore", 1, 150),
                new Resource("Copper Ore", 1, 200),
                new Resource("Titanium Ore", 1, 250),
                new Resource("Gold Ore", 1, 1000),

                new Resource("Iron", 1, 200),
                new Resource("Steel", 1, 300),
                new Resource("Copper", 1, 400),
                new Resource("Titanium", 1, 500),
                new Resource("Gold", 1, 5000),

                new Resource("Silicon", 1, 250),

                new Resource("Processor", 1, 2000),

                new Resource("Basic Ship Components", 10, 6000),
                new Resource("Advanced Components", 1, 10000)
            };
        }

        private void StartMatchingOrders()
        {
            InvokeRepeating("MatchOrders", 1f, 1f);
        }

        /// <summary>
        /// Allows the Unity Editor to call this method.  Don't use this unless you know what you're doing.
        /// </summary>
        public void MatchOrders()
        {
            StartCoroutine(MatchOrdersCoroutine());
        }

        private DateTime orderMatchingStartTime = DateTime.Now;
        public IEnumerator MatchOrdersCoroutine()
        {
            float dt = Time.unscaledDeltaTime;
            orderMatchingStartTime = System.DateTime.Now;

            List<MarketOrder> buyOrdersFilled = new List<MarketOrder>();
            List<MarketOrder> sellOrdersFilled = new List<MarketOrder>();
            List<MarketOrder> resultOrders = new List<MarketOrder>();

            /* For each buy order:
             *     1. Get a list of sell orders that are selling items of the type we're trying to buy.
             *     2. Sort that list by distance from the buyer.
             *     3. Fill as much of the buy order as possible, until either there's no more sell orders or the buy order is filled
             *     4. Remove the filled buy order from the list of buy orders, turn the consumed sell orders into delivery jobs
            */
            List<MarketOrder> sellingOrders = new List<MarketOrder>();
            List<MarketOrder> buyingOrders = new List<MarketOrder>();

            foreach (Planet p in sellingOrdersByPlanet.Keys)
            {
                sellingOrders.AddRange(sellingOrdersByPlanet[p]);
            }
            foreach (Planet p in buyingOrdersByPlanet.Keys)
            {
                buyingOrders.AddRange(buyingOrdersByPlanet[p]);
            }

            foreach (MarketOrder buyOrder in buyingOrders)
            {
                List<MarketOrder> candidates = new List<MarketOrder>();
                foreach (MarketOrder sellOrder in sellingOrders)
                {
                    if (sellOrder.origin != buyOrder.origin)
                    {
                        if (sellOrder.item.KindEquals(buyOrder.item))
                        {
                            candidates.Add(sellOrder);
                        }
                    }
                }

                // Sort the candidates by the distance from the buyer
                // TODO: calculate price modified by distance instead
                MarketOrderComparer comparer = new MarketOrderComparer(buyOrder.origin);
                candidates.Sort(comparer);

                var queue = new Queue<MarketOrder>(candidates);
                MarketOrder currentBuyOrder = buyOrder;
                while (currentBuyOrder.Done != true && queue.Count > 0)
                {
                    MarketOrder currentSellOrder = queue.Dequeue();
                    MarketOrder result = currentBuyOrder.ApplySellOrder(currentSellOrder);
                    resultOrders.Add(result);
                    if (currentSellOrder.Done)
                    {
                        sellOrdersFilled.Add(currentSellOrder);
                    }
                }

                foreach (MarketOrder sold in sellOrdersFilled)
                {
                    sellingOrdersByPlanet[sold.origin].Remove(sold);
                }
                sellOrdersFilled.Clear();
                if (currentBuyOrder.Done)
                {
                    buyOrdersFilled.Add(currentBuyOrder);
                }

                double timeElapsed = (DateTime.Now - orderMatchingStartTime).TotalSeconds;
                if (timeElapsed >= 0.5f * dt)
                {
                    yield return null;
                }
            }

            foreach (MarketOrder bought in buyOrdersFilled)
            {
                buyingOrdersByPlanet[bought.origin].Remove(bought);
            }
            buyOrdersFilled.Clear();

            foreach (MarketOrder result in resultOrders)
            {
                result.SendToPlanet();
            }
        }
    }
}
