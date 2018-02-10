using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Assets.Behavior_Designer.Runtime;
using Assets.Behavior_Designer.Runtime.Variables;
using Assets.Scripts.Classes.WorldSingleton;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Behavior_Designer.Runtime.Actions.Custom
{
    [TaskCategory("Custom/Spaceship")]
    class DropOffResources : Action
    {
        public SharedSpaceship SpaceshipScript;
        public SharedStringList MiningTargetsList;
        public SharedMarketOrder DeliveryOrder;
        public SharedPlanet DropOffDestination;

        private Spaceship shipScript;

        public override void OnStart()
        {
            shipScript = SpaceshipScript.Value;
        }

        public override TaskStatus OnUpdate()
        {
            List<global::Planet> planets = shipScript.GetInInteractionRange<global::Planet>();
            if (planets.Contains(DropOffDestination.Value))
            {
                if (MiningTargetsList.Value.Count != 0)
                {
                    List<string> miningTargetsList = MiningTargetsList.Value;
                    foreach (string resource in miningTargetsList)
                    {
                        DropOffDestination.Value.GetCargoHold.Credit(resource, shipScript.GetCargoHold, shipScript.GetCargoHold.GetAmountInHold(resource), true);
                    }
                }
                else if (DeliveryOrder.Value != null)
                {
                    string type = DeliveryOrder.Value.item.Name;
                    DeliveryOrder.Value.Succeed();
                    DropOffDestination.Value.GetCargoHold.Credit(type, shipScript.GetCargoHold, shipScript.GetCargoHold.GetAmountInHold(type), true);
                }
            }
            return TaskStatus.Success;
        }
    }
}
