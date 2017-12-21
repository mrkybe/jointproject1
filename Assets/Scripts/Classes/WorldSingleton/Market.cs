using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Classes.WorldSingleton
{
    public partial class Overseer : Static.Static
    {
        public List<MarketOrder> sellingOrders;
        public List<MarketOrder> buyingOrders;
        public Dictionary<Planet, MarketOrder> executedOrders;

        private void CreateMarket()
        {
            sellingOrders = new List<MarketOrder>();
            buyingOrders = new List<MarketOrder>();
            executedOrders = new Dictionary<Planet, MarketOrder>();
        }

        public void PlaceSellOrder(MarketOrder order)
        {
            sellingOrders.Add(order);
        }

        public void PlaceBuyOrder(MarketOrder order)
        {
            buyingOrders.Add(order);
        }
        
        public void MatchOrders()
        {
            List<MarketOrder> buyOrdersFilled = new List<MarketOrder>();
            List<MarketOrder> sellOrdersFilled = new List<MarketOrder>();
            List<MarketOrder> resultOrders = new List<MarketOrder>();

            /* For each buy order:
             *     1. Get a list of sell orders that are selling items of the type we're trying to buy.
             *     2. Sort that list by distance from the buyer.
             *     3. Fill as much of the buy order as possible, until either there's no more sell orders or the buy order is filled
             *     4. Remove the filled buy order from the list of buy orders, turn the consumed sell orders into delivery jobs
            */
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
                    sellingOrders.Remove(sold);
                }
                sellOrdersFilled.Clear();
                if (currentBuyOrder.Done)
                {
                    buyOrdersFilled.Add(currentBuyOrder);
                }
            }

            foreach (MarketOrder bought in buyOrdersFilled)
            {
                buyingOrders.Remove(bought);
            }
            buyOrdersFilled.Clear();

            foreach (MarketOrder result in resultOrders)
            {
                result.SendToPlanet();
            }
        }
    }
}
