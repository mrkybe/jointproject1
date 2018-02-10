using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Classes.Static;
using BehaviorDesigner.Runtime;

namespace Assets.Behavior_Designer.Runtime.Variables
{
    class SharedAsteroidList : SharedVariable<List<AsteroidField>>
    {
        public static implicit operator SharedAsteroidList(List<AsteroidField> value)
        {
            return new SharedAsteroidList { Value = value };
        }
    }
}