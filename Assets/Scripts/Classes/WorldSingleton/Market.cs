using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void StartMatchingOrders()
        {
            //InvokeRepeating("MatchOrders", 1f, 1f);
        }

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
