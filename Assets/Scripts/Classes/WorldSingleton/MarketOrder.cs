using System;
using System.Collections.Generic;
using NUnit.Framework;
using ShipInternals;
using UnityEngine;

namespace Assets.Scripts.Classes.WorldSingleton
{
    public class MarketOrder
    {
        public readonly Planet origin;
        public CargoItem item;
        public Planet destination = null;
        public bool Done = false;

        public MarketOrder(Planet origin, CargoItem item)
        {
            this.origin = origin;
            this.item = item;
        }

        public MarketOrder(Planet origin, Planet destination, CargoItem item)
        {
            this.origin = origin;
            this.item = item;
            this.destination = destination;
        }

        // returns a MarketOrder that is sent to a Planet to be delivered.
        // checks if this order is Done and if the sellOrder is done.
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

        public void SendToPlanet()
        {
            origin.AddToDeliveryQueue(this);
        }
    }

    public class MarketOrderComparer : IComparer<MarketOrder>
    {
        private Planet buyer;

        public MarketOrderComparer(Planet buyer)
        {
            this.buyer = buyer;    
        }

        public int CompareClosest(MarketOrder x, MarketOrder y)
        {
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
