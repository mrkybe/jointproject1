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
    class UpdateSafety : Action
    {
        public SharedSpaceship SpaceshipScript;
        public SharedVector3 FleeDirection;

        public override TaskStatus OnUpdate()
        {
            int fear_level = 0;
            List<Spaceship> scaryList = new List<Spaceship>();
            var hostile = GetHostileShipsInRange();
            foreach (Spaceship f in hostile)
            {
                // add to fear level only positive values, since weak ships shouldn't make you fight a carrier
                fear_level += Mathf.Clamp(f.GetScaryness(SpaceshipScript.Value), 0, int.MaxValue);
                scaryList.Add(f);
            }

            Vector3 averageScaryPosition = Vector3.zero;
            foreach (Spaceship f in scaryList)
            {
                averageScaryPosition += f.transform.position;
            }
            averageScaryPosition /= scaryList.Count;
            
            if (scaryList.Count > 0)
            {
                FleeDirection.Value = ((averageScaryPosition - transform.position) * -1).normalized;
            }
            else
            {
                FleeDirection.Value = Vector3.zero;
            }
            //BehaviorTree.Blackboard["fearLevel"] = fear_level;
            return TaskStatus.Success;
        }

        List<Spaceship> GetHostileShipsInRange()
        {
            Faction myFaction = SpaceshipScript.Value.Faction;
            List<Spaceship> resultsList = new List<Spaceship>();

            foreach (Spaceship f in SpaceshipScript.Value.GetInSensorRange<Spaceship>())
            {
                if (f.Faction.HostileWith(myFaction) && f.Alive)
                {
                    resultsList.Add(f);
                }
            }

            return resultsList;
        }
    }
}
