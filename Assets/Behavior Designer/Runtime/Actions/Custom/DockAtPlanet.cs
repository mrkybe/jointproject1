using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Basic.UnityGameObject;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Behavior_Designer.Runtime.Actions.Custom
{
    class DockAtPlanet : Action
    {
        public SharedSpaceship SpaceshipScript;
        public SharedPlanet TargetPlanet;
        public SharedBool LandInstead = false;

        public override TaskStatus OnUpdate()
        {
            if (LandInstead.Value == false)
            {
                List<global::Planet> planets = SpaceshipScript.Value.GetInInteractionRange<global::Planet>();
                if (planets.Contains(TargetPlanet.Value))
                {
                    TargetPlanet.Value.AddToAvailableDeliveryShips((AI_Patrol)SpaceshipScript.Value.GetPilot);
                    return TaskStatus.Success;
                }
                return TaskStatus.Failure;
            }
            else
            {
                List<global::Planet> planets = SpaceshipScript.Value.GetInInteractionRange<global::Planet>();
                if (planets.Contains(TargetPlanet.Value))
                {
                    TargetPlanet.Value.ReturnDeliveryShip((AI_Patrol)SpaceshipScript.Value.GetPilot);
                    GameObject.Destroy(SpaceshipScript.Value.gameObject);
                    return TaskStatus.Success;
                }
                return TaskStatus.Failure;
            }
        }
    }
}
