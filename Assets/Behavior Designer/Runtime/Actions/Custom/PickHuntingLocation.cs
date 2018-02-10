using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Behavior_Designer.Runtime.Actions.Custom
{
    class PickHuntingLocation : Action
    {
        public SharedVector3 Target;
        private List<Vector3> PreviousPositions;

        public override void OnAwake()
        {
            base.OnAwake();
            PreviousPositions = new List<Vector3>();
        }

        public override TaskStatus OnUpdate()
        {
            List<Vector3> huntingAreas = GetHuntingAreas();
            Vector3 myPos = transform.position;
            Vector3 closestPos = huntingAreas[0];
            float minDistance = float.MaxValue;
            foreach (Vector3 huntingPos in huntingAreas)
            {
                if (Vector3.Distance(huntingPos, myPos) < minDistance)
                {
                    minDistance = Vector3.Distance(huntingPos, myPos);
                    closestPos = huntingPos;
                }
            }
            Target.Value = closestPos;
            PreviousPositions.Add(closestPos);
            return TaskStatus.Success;
        }

        /// <summary>
        /// Returns a list of mid points between planets that are close to each other.
        /// </summary>
        /// <returns></returns>
        private List<Vector3> GetHuntingAreas()
        {
            List<Vector3> huntingPositions = new List<Vector3>();
            foreach (global::Planet p in global::Planet.listOfPlanetObjects)
            {
                List<global::Planet> nearestPlanets = new List<global::Planet>(global::Planet.listOfPlanetObjects);
                nearestPlanets.Remove(p);
                global::Planet.PlanetComparer sortComparer = new global::Planet.PlanetComparer(p);
                nearestPlanets.Sort(sortComparer);
                for (int i = 0; i < 3 && i < nearestPlanets.Count; i++)
                {
                    Vector3 posA = p.transform.position;
                    Vector3 posB = nearestPlanets[i].transform.position;
                    Vector3 newPosition = (posA + posB) / 2;
                    huntingPositions.Add(newPosition);
                }
            }
            if (PreviousPositions.Count < huntingPositions.Count / 2)
            {
                foreach (Vector3 pos in PreviousPositions)
                {
                    huntingPositions.Remove(pos);
                }
            }
            else
            {
                PreviousPositions.Clear();
            }
            return huntingPositions;
        }
    }
}
