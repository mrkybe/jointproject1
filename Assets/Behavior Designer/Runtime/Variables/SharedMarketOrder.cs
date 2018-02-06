using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Classes.WorldSingleton;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Variables
{
    class SharedMarketOrder : SharedVariable<MarketOrder>
    {
        public static implicit operator SharedMarketOrder(MarketOrder value)
        {
            return new SharedMarketOrder { Value = value };
        }
    }
}
