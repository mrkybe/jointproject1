using System;
using System.Collections.Generic;
using Assets.Scripts.Classes.Helper.ShipInternals;
using Assets.Scripts.Classes.Mobile;
using Assets.Scripts.Classes.Static;
using UnityEngine;

namespace Assets.Scripts.Classes.WorldSingleton
{
    /// <summary>
    /// An order to buy or sell for a given resource, by who, for whom, completion status.
    /// </summary>
    [Serializable]
    public class MarketOrder
    {
        public readonly Planet origin;
        public Planet destination = null;
        public CargoItem item;
        public bool Done = false;
        public Spaceship ship = null;
        public enum OrderStatus { FAIL_SHIP_DEAD, FAIL_NO_MONEY, IN_PROGRESS, SUCCEED}
        private OrderStatus orderStatus = OrderStatus.IN_PROGRESS;

        private MarketOrder()
        {

        }

        /// <summary>
        /// A Buy or Sell order.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="item"></param>
        public MarketOrder(Planet origin, CargoItem item)
        {
            this.origin = origin;
            this.item = item;
        }

        /// <summary>
        /// A Market order that is to be carried out.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <param name="item"></param>
        public MarketOrder(Planet origin, Planet destination, CargoItem item)
        {
            this.origin = origin;
            this.item = item;
            this.destination = destination;
        }
        
        /// <summary>
        /// Applys sell order to this MarketOrder.  Checks if this order is Done and if the sellOrder is also done.
        /// </summary>
        /// <param name="sellOrder">The sell order to apply.</param>
        /// <returns>Returns a MarketOrder that is sent to a Planet to be delivered.</returns>
        public MarketOrder ApplySellOrder(MarketOrder sellOrder)
        {
            int buyAmount = item.Count;
            int sellAmount = sellOrder.item.Count;
            if (buyAmount > sellAmount)
            {
                item.Count -= sellAmount;
                sellOrder.Done = true;
                return new MarketOrder(sellOrder.origin, this.origin, sellOrder.item.Copy());
            }
            else if (buyAmount == sellAmount)
            {
                item.Count -= sellAmount;
                sellOrder.Done = true;
                this.Done = true;
                return new MarketOrder(sellOrder.origin, this.origin, sellOrder.item.Copy());
            }
            else// if(buyAmount < sellAmount)
            {
                sellOrder.item.Count -= buyAmount;
                this.Done = true;
                return new MarketOrder(sellOrder.origin, this.origin, this.item.Copy());
            }
        }

        /// <summary>
        /// Sends this order to the planet it came from.
        /// </summary>
        public void SendToPlanet()
        {
            origin.AddToDeliveryQueue(this);
        }

        /// <summary>
        /// Combines this order with another order.
        /// </summary>
        /// <param name="order"></param>
        public void Combine(MarketOrder order)
        {
            item.Count += order.item.Count;
        }

        /// <summary>
        /// Called when the order is completed (including delivery).
        /// </summary>
        public void Succeed()
        {
            orderStatus = OrderStatus.SUCCEED;
            origin.CompleteOrder(this);
        }

        /// <summary>
        /// Called when the order fails.
        /// </summary>
        public void Fail(OrderStatus cause)
        {
            orderStatus = cause;
            origin.FailOrder(this);
        }

        /// <summary>
        /// Returns the status of the order.
        /// </summary>
        public OrderStatus GetOrderStatus()
        {
            return orderStatus;
        }
    }

    /// <summary>
    /// Compares market orders with regards to a Planet.
    /// </summary>
    [Serializable]
    public class MarketOrderComparer : IComparer<MarketOrder>
	{
        private Planet buyer;

        public MarketOrderComparer(Planet buyer)
        {
            this.buyer = buyer;    
        }

        public int CompareClosest(MarketOrder x, MarketOrder y)
        {
            if (x.origin == null || y.origin == null)
            {
                Debug.Log("waat");
                return 1;
            }
            Vector3 xPos = x.origin.transform.position;
            Vector3 yPos = y.origin.transform.position;
            Vector3 buyerPos = buyer.transform.position;
            float xDist = Vector3.Distance(buyerPos, xPos);
            float yDist = Vector3.Distance(buyerPos, yPos);
            if (xDist < yDist)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        public int Compare(MarketOrder x, MarketOrder y)
        {
            return CompareClosest(x,y);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
