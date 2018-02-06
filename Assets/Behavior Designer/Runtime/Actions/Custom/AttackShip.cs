using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Behavior_Designer.Runtime.Variables;
using Assets.Scripts.Classes.WorldSingleton;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Behavior_Designer.Runtime.Actions.Custom
{
    class AttackShip : Action
    {
        public SharedSpaceship AttackTarget;
        public SharedSpaceship SpaceshipScript;

        public override TaskStatus OnUpdate()
        {
            Overseer.BattleResult result = Overseer.Main.ResolveShipCombat(SpaceshipScript.Value, AttackTarget.Value);
            return TaskStatus.Success;
        }
    }
}
