using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityParts
{
    /* Written by Ruslan Kaybyshev, Late 2013
     * Purpose: One centralized place where data structure type classes can be
     *          stored easily.  These data structure types (Entity Components)
     *          can be used to easily add common functionality to any
     *          components that are implimented later.
     */
    public class CargoHold
    {
        int maxHold;
        List<CargoBay> holdTypes;

        public CargoHold()
        {

        }
    }

    public class CargoBay
    {
        int amount;
        CargoItem type;

        public CargoBay()
        {

        }
    }

    public class CargoItem
    {
        string type; // use something better than a string for this, data driven ideally, ie: xml file somewhere that has all of the resource definitions and is parsed into cargo item types.
        int size;

        public CargoItem()
        {

        }
    }
}
