using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityParts
{
    public class CargoHold
    {
        int maxHold;
        List<CargoBay> holdTypes;

    }

    public class CargoBay
    {
        int amount;
        CargoItem type; // use something better than a string for this
    }

    public class CargoItem
    {
        string type; // use something better than a string for this
        int size;
    }
}
