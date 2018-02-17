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
    /// <summary>
    /// Action to Attack a target Spaceship.
    /// </summary>
    class ScrapShip : Action
    {
        public SharedSpaceship AttackTarget;

        public override TaskStatus OnUpdate()
        {
            AttackTarget.Value.DeleteSelf();
            return TaskStatus.Success;
        }
    }
}
