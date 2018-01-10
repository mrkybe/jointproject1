using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI_Missions;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.WorldSingleton;
using NPBehave;
using ShipInternals;
using Action = NPBehave.Action;

public class AI_Patrol : PilotInterface
{
    private Blackboard blackboard;
    private Spaceship shipScript;
    private Debugger debugger = null;

    // Use this for initialization
    void Awake()
    {
        shipScript = transform.GetComponent<Spaceship>();

#if UNITY_EDITOR
        if (debugger == null)
        {
            debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        }
#endif
    }

    void Start()
    {
        base.Start();
    }

    public void StartMining(string targetName)
    {
        behaviorTree = CreateBehaviourTreeDumbMining();

        blackboard = behaviorTree.Blackboard;
        blackboard["miningTarget"] = targetName;

        // temporarily use this as the home base until we have a better system
        BlackboardSetNearestPlanet();

#if UNITY_EDITOR
        if (debugger == null)
        {
            debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        }
        debugger.BehaviorTree = behaviorTree;
#endif
        behaviorTree.Start();
    }

    public void StartDelivery(MarketOrder order)
    {
        if (behaviorTree != null)
        {
            behaviorTree.Stop();
        }
        else
        {
            behaviorTree = CreateBehaviourTreeDumbDelivery();
            blackboard = behaviorTree.Blackboard;
        }

        blackboard["deliveryOrder"] = order;
        blackboard["homePlanet"] = order.origin;
        blackboard["deliveryPlanet"] = order.destination;

#if UNITY_EDITOR
        if (debugger == null)
        {
            debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        }
        debugger.BehaviorTree = behaviorTree;
#endif
        behaviorTree.Start();
    }

    private Root CreateBehaviorTreePirate()
    {
        return new Root(
            new Sequence(
                new Wait(1f),
                new Action(() =>
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
                        blackboard.Set("targetPos", closestPos);
                    })
                { Label = "Set target position to nearest hunting area" },
                new Service(0.5f, UpdateDistanceToTarget,
                    new Action((bool shouldCancel) =>
                        {
                            if (!shouldCancel)
                            {
                                MoveTowards(blackboard.Get<Vector3>("targetPos"));
                                if (blackboard.Get<float>("targetDistance") < 5f)
                                {
                                    return Action.Result.SUCCESS;
                                }
                                return Action.Result.PROGRESS;
                            }
                            else
                            {
                                return Action.Result.FAILED;
                            }
                        })
                    { Label = "Go to target" }
                    )
            )
        );
    }

    private List<Vector3> GetHuntingAreas()
    {
        List<Vector3> huntingPositions = new List<Vector3>();
        foreach (Planet p in Planet.listOfPlanetObjects)
        {
            List<Planet> nearestPlanets = new List<Planet>(Planet.listOfPlanetObjects);
            nearestPlanets.Remove(p);
            Planet.PlanetComparer sortComparer = new Planet.PlanetComparer(p);
            nearestPlanets.Sort(sortComparer);
            for (int i = 0; i < 3 && i < nearestPlanets.Count; i++)
            {
                Vector3 posA = p.transform.position;
                Vector3 posB = nearestPlanets[i].transform.position;
                Vector3 newPosition = (posA + posB) / 2;
                huntingPositions.Add(newPosition);
            }
        }
        return huntingPositions;
    }

    private Root CreateBehaviourTreeDumbDelivery()
    {
        return new Root(
            new Sequence(
                new Wait(1f),
                new Action(() =>
                    {
                        targetPosition = blackboard.Get<Planet>("deliveryPlanet").transform.position;
                    })
                { Label = "Set target position to go to drop off planet" },
                new Service(0.5f, UpdateDistanceToTarget,
                    new Action((bool shouldCancel) =>
                        {
                            if (!shouldCancel)
                            {
                                MoveTowards(blackboard.Get<Vector3>("targetPos"));
                                if (blackboard.Get<float>("targetDistance") < 5f)
                                {
                                    return Action.Result.SUCCESS;
                                }
                                return Action.Result.PROGRESS;
                            }
                            else
                            {
                                return Action.Result.FAILED;
                            }
                        })
                    { Label = "Go to target" }
                ),
                new Action(() =>
                    {
                        DropOffResource(blackboard.Get<MarketOrder>("deliveryOrder"));
                    })
                { Label = "Dropping off resources" },
                new Action(() =>
                    {
                        targetPosition = blackboard.Get<Planet>("homePlanet").transform.position;
                    })
                { Label = "Set target position to go home" },
                new Service(0.5f, UpdateDistanceToTarget,
                    new Action((bool shouldCancel) =>
                        {
                            if (!shouldCancel)
                            {
                                MoveTowards(blackboard.Get<Vector3>("targetPos"));
                                if (blackboard.Get<float>("targetDistance") < 5f)
                                {
                                    return Action.Result.SUCCESS;
                                }
                                return Action.Result.PROGRESS;
                            }
                            else
                            {
                                return Action.Result.FAILED;
                            }
                        })
                    { Label = "Go to target planet" }
                ),
                new Action(() =>
                    {
                        Planet nearest = GetNearestPlanet();
                        nearest.AddToAvailableDeliveryShips(this);
                        blackboard["arriveTime"] = Time.time;
                    })
                { Label = "Dock with planet" },
                new Service(0.5f, UpdateDistanceToTarget,
                    new Action((bool shouldCancel) =>
                        {
                            if (!shouldCancel)
                            {
                                MoveTowards(blackboard.Get<Vector3>("targetPos"));
                                if (Time.time - blackboard.Get<float>("arriveTime") > 5f)
                                {
                                    //Debug.Log("ARRIVED!!!! " + (Time.time - blackboard.Get<float>("arriveTime")));
                                    return Action.Result.SUCCESS;
                                }
                                return Action.Result.PROGRESS;
                            }
                            else
                            {
                                return Action.Result.FAILED;
                            }
                        })
                    { Label = "Wait at target position for 5 seconds" }
                ),
                new Action(() =>
                    {
                        Planet nearest = GetNearestPlanet();
                        nearest.ReturnDeliveryShip(this);
                        Destroy(this.gameObject);
                    })
                { Label = "Dock with planet" }
            )
        );
    }

    public void OnDestroy()
    {
        behaviorTree.Stop();
    }

    private Root CreateBehaviourTreeDumbMining()
    {
        return new Root(
            new Sequence(
                    new Wait(1f),
                    new Action(() =>
                               {
                                   FindNearestAsteroidField();
                                   targetPosition = blackboard.Get<AsteroidField>("nearestAsteroidField").transform.position;
                               })
                    { Label = "Find Nearest Asteroid Field" },
                    new Service(0.5f, UpdateDistanceToTarget,
                        new Action((bool shouldCancel) =>
                                   {
                                       if (!shouldCancel)
                                       {
                                           MoveTowards(blackboard.Get<Vector3>("targetPos"));
                                           if (blackboard.Get<float>("targetDistance") < 5f)
                                           {
                                               return Action.Result.SUCCESS;
                                           }
                                           return Action.Result.PROGRESS;
                                       }
                                       else
                                       {
                                           return Action.Result.FAILED;
                                       }
                                   })
                        { Label = "Go to target" }
                    ),
                    new Succeeder(new Repeater(new Cooldown(0.1f,
                        new Action((bool shouldCancel) =>
                                   {
                                       control_stickDirection = new Vector2();
                                       targetSpeed = 0;
                                       int spaceRemaining = shipScript.GetCargoHold.GetRemainingSpace();
                                       if (spaceRemaining == 0)
                                       {
                                           blackboard.Set("cargoHoldFull", true);
                                       }
                                       int mined = Mine("Gold");
                                       if (mined > 0)
                                       {
                                           return Action.Result.SUCCESS;
                                       }
                                       else
                                       {
                                           return Action.Result.FAILED; // this should search for minable asteroid fields better
                                       }
                                   })
                        { Label = "Mining" }
                    ))),
                    new Action(() =>
                               {
                                   targetPosition = blackboard.Get<Planet>("homePlanet").transform.position;
                               })
                    { Label = "Set target position to go home" },
                    new Service(0.5f, UpdateDistanceToTarget,
                        new Action((bool shouldCancel) =>
                                    {
                                        if (!shouldCancel)
                                        {
                                            MoveTowards(blackboard.Get<Vector3>("targetPos"));
                                            if (blackboard.Get<float>("targetDistance") < 5f)
                                            {
                                                return Action.Result.SUCCESS;
                                            }
                                            return Action.Result.PROGRESS;
                                        }
                                        else
                                        {
                                            return Action.Result.FAILED;
                                        }
                                    })
                        { Label = "Go to target planet" }
                        ),
                    new Action(() =>
                               {
                                   DropOffResource("Gold");
                                   FindNearestAsteroidField();
                               })
                    { Label = "Dropping off resources" }
                    )
        );
    }

    private void FindNearestAsteroidField()
    {
        float nearestDistance = float.MaxValue;
        bool found = false;

        // faster to use optimized physics engine and narrow down our list of objects to check
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 100f, Vector3.up);

        if (hits.Length > 0)
        {
            //Debug.Log("Checking Hits!");
            foreach (var hit in hits)
            {
                AsteroidField one = hit.transform.gameObject.GetComponent<AsteroidField>();
                if (one)
                {
                    if ((transform.position - one.transform.position).magnitude < nearestDistance)
                    {
                        string miningTarget = blackboard.Get<String>("miningTarget");
                        if (one.GetCargoHold.Contains(miningTarget) && one.GetCargoHold.GetAmountInHold(miningTarget) > 0)
                        {
                            blackboard["nearestAsteroidField"] = one;
                            nearestDistance = (transform.position - one.transform.position).magnitude;
                            targetPosition = one.transform.position;
                            found = true;
                        }
                    }
                }
            }
        }
        if (!found)
        {
            //Debug.Log("Checking All Asteroids!");
            foreach (var one in AsteroidField.listOfAsteroidFields)
            {
                if ((transform.position - one.transform.position).magnitude < nearestDistance)
                {
                    string miningTarget = blackboard.Get<String>("miningTarget");
                    if (one.GetCargoHold.Contains(miningTarget) && one.GetCargoHold.GetAmountInHold(miningTarget) > 0)
                    {
                        blackboard["nearestAsteroidField"] = one;
                        nearestDistance = (transform.position - one.transform.position).magnitude;
                        targetPosition = one.transform.position;
                    }
                }
            }
        }
    }

    private void BlackboardSetNearestPlanet()
    {
        blackboard.Set("homePlanet", GetNearestPlanet());
    }

    private Planet GetNearestPlanet()
    {
        Planet bestPlanet = null;
        float nearestDistance = float.MaxValue;
        foreach (var one in Planet.listOfPlanetObjects)
        {
            if ((transform.position - one.transform.position).magnitude < nearestDistance)
            {
                bestPlanet = one;
                nearestDistance = (transform.position - one.transform.position).magnitude;
            }
        }
        return bestPlanet;
    }

    private void UpdateCargoHoldSpaceRemaining()
    {
        blackboard["cargoSpaceRemaining"] = shipScript.GetCargoHold.GetRemainingSpace();
    }

    Vector3 targetPosition = Vector3.zero;
    private void UpdateDistanceToTarget()
    {
        Vector3 targetPos = targetPosition;
        behaviorTree.Blackboard["targetPos"] = targetPos;
        Vector3 targetLocalPos = targetPosition - this.transform.position;
        behaviorTree.Blackboard["targetDistance"] = targetLocalPos.magnitude;
    }

    private void UpdateSafetyStatus()
    {
        Faction myFaction = shipScript.Faction;
        foreach (Spaceship f in shipScript.GetShipsInRange())
        {
            if (f.Faction.HostileWith(myFaction))
            {
                
            }
        }
    }

    private void MoveTowards(Vector3 position)
    {
        Vector2 stick = new Vector2();
        float targetAngle = Vector3.Angle((transform.forward).normalized, (position - transform.position).normalized);
        if (isLeft(transform.position, transform.position + transform.forward * 500, position))
        {
            targetAngle = -targetAngle;
        }

        targetSpeed = Mathf.Clamp((Mathf.Clamp01(1 - (Mathf.Abs(targetAngle) / 60)) * 3), 0f, 10f);

        //Debug.Log(targetSpeed);
        Vector3 temp = position - transform.position;

        // TODO:  Make this more accurate by using _parent.transform.forward - target position.
        /*if (temp.magnitude < mySensorArray.StoppingDistance)
        {
            bool slowDown = true;
        }*/
        //temp = temp.normalized + _parent.transform.forward;
        stick = new Vector2(temp.x, temp.z);
        Debug.DrawLine(transform.position, position);
        Debug.DrawLine(transform.position, transform.position + transform.forward);
        stick.y = 0;
        stick.x = targetAngle / 10;

        stick.x = Mathf.Clamp(stick.x, -1f, 1f);
        stick.y = Mathf.Clamp(stick.y, -1f, 1f);

        control_stickDirection = stick;
    }

    private int Mine(String miningTarget)
    {
        int mineAmount = 1;
        int minedAmount = 0;
        if (shipScript != null)
        {
            List<Static> targets = shipScript.GetStaticInRange();
            List<AsteroidField> newTargets = new List<AsteroidField>();
            AsteroidField finalTarget;
            Debug.Log("Targets available: " + targets.Count);
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].GetType() == typeof(AsteroidField))
                {
                    finalTarget = targets[i] as AsteroidField;
                    if (finalTarget.GetCargoHold.Contains(miningTarget) &&
                        finalTarget.GetCargoHold.GetAmountInHold(miningTarget) > 0)
                    {
                        newTargets.Add((AsteroidField)targets[i]);
                    }
                }

            }
            if (newTargets.Count >= 1)
            {
                finalTarget = newTargets[0];
                minedAmount = shipScript.GetCargoHold.Credit(miningTarget, finalTarget.GetCargoHold, mineAmount);
                Debug.Log("Taking Cargo");
            }
        }

        return minedAmount;
    }

    private int DropOffResource(String type)
    {
        return GetNearestPlanet().GetCargoHold.Credit(type, shipScript.GetCargoHold, shipScript.GetCargoHold.GetAmountInHold(type), true);
    }

    private void DropOffResource(MarketOrder type)
    {
        DropOffResource(type.item.Name);
        type.Succeed();
        //throw new NotImplementedException();
    }

    public bool isLeft(Vector3 pos1, Vector3 pos2, Vector3 checkPoint)
    {
        return ((pos2.x - pos1.x) * (checkPoint.z - pos1.z) - (pos2.z - pos1.z) * (checkPoint.x - pos1.x)) > 0;
    }
}
