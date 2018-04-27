using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Behavior_Designer.Runtime.Variables;
using Assets.Scripts.Classes.Helper.Pilot;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Basic.UnityGameObject;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Behavior_Designer.Runtime.Actions.Custom
{
    class WriteToShipLog : Action
    {
        public SharedSpaceship SpaceshipScript;
        public SharedSpaceship AttackTargetScript; // [1] Ship Name, [2] Pilot Name
        public SharedPlanet OriginPlanet;          // [3] Planet Name
        public SharedPlanet TargetPlanet;          // [4] Planet Name
        public SharedMarketOrder MarketOrder;      // [5] Item Name, [6] Item Amount
        public SharedString Message;

        public override TaskStatus OnUpdate()
        {
            string finalMessage = Message.Value;
            if (AttackTargetScript.Value != null)
            {
                finalMessage = finalMessage.Replace("[1]", AttackTargetScript.Value.gameObject.name);
                finalMessage = finalMessage.Replace("[2]", AttackTargetScript.Value.Pilot.Identity.ToString());
            }

            if (OriginPlanet.Value != null)
            {
                finalMessage = finalMessage.Replace("[3]", OriginPlanet.Value.MyName);
            }

            if (TargetPlanet.Value != null)
            {
                finalMessage = finalMessage.Replace("[4]", TargetPlanet.Value.MyName);
            }

            if (MarketOrder.Value != null)
            {
                finalMessage = finalMessage.Replace("[5]", MarketOrder.Value.item.Name);
                finalMessage = finalMessage.Replace("[6]", MarketOrder.Value.item.Count.ToString());
            }

            //Debug.Log(finalMessage);
            SpaceshipScript.Value.BlackBox.Write(finalMessage, this.transform.position);
            return TaskStatus.Success;
        }
    }
}