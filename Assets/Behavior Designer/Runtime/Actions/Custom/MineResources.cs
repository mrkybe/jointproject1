using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Assets.Behavior_Designer.Runtime;
using Assets.Behavior_Designer.Runtime.Variables;
using Assets.Scripts.Classes.Mobile;
using Assets.Scripts.Classes.Static;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Behavior_Designer.Runtime.Actions
{
    [TaskCategory("Custom/Spaceship")]
    class MineResources : Action
    {
        public SharedSpaceship SpaceshipScript;
        public SharedStringList MiningTargetsList;
        public SharedAsteroid AsteroidToMine;

        private Spaceship shipScript;

        public override void OnStart()
        {
            shipScript = SpaceshipScript.Value;
        }

        public override TaskStatus OnUpdate()
        {
            int spaceRemaining = shipScript.GetCargoHold.GetRemainingSpace();
            if (spaceRemaining != 0)
            {
                int mined = Mine(MiningTargetsList.Value);
                if (mined > 0)
                {
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Failure; // this should search for minable asteroid fields better
        }

        private int Mine(List<string> list)
        {
            int maxMineAmount = 1;
            int minedAmount = 0;
            foreach (string resource in list)
            {
                if (minedAmount < maxMineAmount)
                {
                    minedAmount += MineResource(resource, maxMineAmount);
                }
                else
                {
                    break;
                }
            }
            return minedAmount;
        }

        private int MineResource(String miningTarget, int maxMineAmount)
        {
            int mineAmount = maxMineAmount;
            int minedAmount = 0;
            if (shipScript != null)
            {
                List<AsteroidField> targets = shipScript.GetInInteractionRange<AsteroidField>();
                AsteroidField finalTarget = AsteroidToMine.Value;
                if (!targets.Contains(finalTarget))
                {
                    Debug.Log("THIS SHOULDN'T HAPPEN!");
                }

                minedAmount = shipScript.GetCargoHold.Credit(miningTarget, finalTarget.CargoHold, mineAmount);
            }

            return minedAmount;
        }
    }
}
