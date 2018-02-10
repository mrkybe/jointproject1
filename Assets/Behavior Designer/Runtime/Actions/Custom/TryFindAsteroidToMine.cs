using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Assets.Behavior_Designer.Runtime;
using Assets.Behavior_Designer.Runtime.Variables;
using Assets.Scripts.Classes.Static;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Scripts.Classes.Helper.Pilot
{
    class TryFindAsteroidToMine : Action
    {
        public SharedAsteroid AsteroidToMine;
        public SharedVector3 TargetPosition;
        public SharedStringList MiningTargetsList;

        public override TaskStatus OnUpdate()
        {
            AsteroidField bestCandidate = FindNearestAsteroidFieldForMining(MiningTargetsList.Value);
            if (bestCandidate != null)
            {
                AsteroidToMine.Value = bestCandidate;
                TargetPosition.Value = bestCandidate.transform.position;
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }

        private AsteroidField FindNearestAsteroidFieldForMining(List<string> miningTargetsList)
        {
            AsteroidField bestCandidate = null;

            // faster to use optimized physics engine and narrow down our list of objects to check
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 100f, Vector3.up);

            if (hits.Length > 0)
            {
                //Debug.Log("Checking Hits!");
                List<AsteroidField> fields = new List<AsteroidField>();
                foreach (var hit in hits)
                {
                    AsteroidField one = hit.transform.gameObject.GetComponent<AsteroidField>();
                    if (one)
                    {
                        fields.Add(one);
                    }
                }

                bestCandidate = FindNearestAsteroidFieldForMiningFromCandidates(fields, miningTargetsList);
            }
            // okay search all of them :(
            if (bestCandidate == null)
            {
                bestCandidate = FindNearestAsteroidFieldForMiningFromCandidates(AsteroidField.listOfAsteroidFields, miningTargetsList);
            }

            return bestCandidate;
        }

        private AsteroidField FindNearestAsteroidFieldForMiningFromCandidates(List<AsteroidField> candidates, List<string> miningTargetsList)
        {
            AsteroidField bestCandidate = null;
            float nearestDistance = float.MaxValue;
            foreach (var one in candidates)
            {
                float distance = (transform.position - one.transform.position).magnitude;
                if (distance < nearestDistance)
                {
                    List<string> found = CheckAsteroidFieldForMinable(one, miningTargetsList);
                    if (found.Count > 0)
                    {
                        nearestDistance = distance;
                        bestCandidate = one;
                    }
                }
            }

            return bestCandidate;
        }

        private List<string> CheckAsteroidFieldForMinable(AsteroidField field, List<string> miningTargetsList)
        {
            List<string> success = new List<string>();
            foreach (string resource in miningTargetsList)
            {
                if (field.CargoHold.Contains(resource) && field.CargoHold.GetAmountInHold(resource) > 0)
                {
                    success.Add(resource);
                }
            }

            return success;
        }
    }
}
