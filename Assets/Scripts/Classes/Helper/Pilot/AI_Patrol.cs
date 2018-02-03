﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI_Missions;
using Assets.Behavior_Designer.Runtime.Variables;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.WorldSingleton;
using ShipInternals;
using BehaviorDesigner.Runtime;

/* AI_Patrol is the AI for the ships/fleets on the Overmap.
 * It contains all of the atomic AI methods that are used by the AI behavior tree.
 * It provides public functions for Factions/Planets to give orders through.
 * These orders are fulfilled by replacing the current BehaviorTree with a new one.
 */

public class AI_Patrol : PilotInterface
{
    public ExternalBehaviorTree ExternalMiningBehaviorTree;
    public ExternalBehaviorTree ExternalDeliveryBehaviorTree;
    public ExternalBehaviorTree ExternalPirateBehaviorTree;

    private SharedBool Alive;
    private SharedBool Safe;
    private SharedVector2 ControlStick;
    private SharedFloat TargetSpeed;
    private SharedPlanet HomePlanet;
    private SharedSpaceship shipScript;

    private BehaviorTree behaviorTree;
    private float interactionDistance = 5f;

    // Use this for initialization
    public void Awake()
    {
        behaviorTree = transform.GetComponent<BehaviorTree>();
        if (!behaviorTree)
        {
            behaviorTree = gameObject.AddComponent<BehaviorTree>();
            behaviorTree.ExternalBehavior = ExternalMiningBehaviorTree;
            behaviorTree.StartWhenEnabled = true;
        }
    }

    private void InitializeBehaviorTreeVariableReferences()
    {
        Alive = (SharedBool)behaviorTree.GetVariable("Alive");
        ControlStick = (SharedVector2)behaviorTree.GetVariable("ControlStick");
        TargetSpeed = (SharedFloat)behaviorTree.GetVariable("TargetSpeed");
        HomePlanet = (SharedPlanet)behaviorTree.GetVariable("HomePlanet");
        shipScript = (SharedSpaceship)behaviorTree.GetVariable("Shipscript");

        shipScript.Value = transform.GetComponent<Spaceship>();
    }

    public new void Start()
    {
        base.Start();
        InitializeBehaviorTreeVariableReferences();
    }

    public new void Update()
    {
        base.Update();
        control_stickDirection = ControlStick.Value;
        targetSpeed = TargetSpeed.Value;
    }

    /// <summary>
    /// Returns the Shipscript that I am the pilot of.
    /// </summary>
    /// <returns></returns>
    public Spaceship GetShip()
    {
        return shipScript.Value;
    }

    /// <summary>
    /// Returns the Behavior Tree script that makes my decisions.
    /// </summary>
    /// <returns></returns>
    public BehaviorTree GetBehaviorTree()
    {
        return behaviorTree;
    }

    /// <summary>
    /// Sets the Behavior Tree the one for mining.
    /// </summary>
    /// <param name="miningTargets">The kind of resources to mine.</param>
    /// <param name="homePlanet">The planet we drop off resources at.</param>
    public void StartMining(List<string> miningTargets, Planet homePlanet)
    {
        behaviorTree.ExternalBehavior = ExternalMiningBehaviorTree;
        InitializeBehaviorTreeVariableReferences();

        HomePlanet.Value = homePlanet;
        behaviorTree.GetVariable("MiningTargets").SetValue(miningTargets);

        behaviorTree.Start();
    }

    /// <summary>
    /// Sets the Behavior Tree the one for delivering an order.
    /// </summary>
    /// <param name="order">The order that the ship is responsible for completing.</param>
    public void StartDelivery(MarketOrder order)
    {
        behaviorTree.ExternalBehavior = ExternalDeliveryBehaviorTree;
        InitializeBehaviorTreeVariableReferences();

        HomePlanet.Value = order.origin;
        behaviorTree.GetVariable("DeliveryOrder").SetValue(order);
        behaviorTree.GetVariable("DeliveryPlanet").SetValue(order.destination);
        behaviorTree.Start();
    }

    /// <summary>
    /// Sets the Behavior Tree to the one for piracy.
    /// </summary>
    public void StartPirate()
    {
        behaviorTree.ExternalBehavior = ExternalPirateBehaviorTree;
        InitializeBehaviorTreeVariableReferences();

        shipScript.Value.Faction = Overseer.Main.GetFaction("Pirates");
        behaviorTree.Start();
    }

    /// <summary>
    /// This kills the Pilot.  Sets a Flag in the Blackboard for being dead,
    /// Behavior Trees must clean up after themselves and go into their dead state.
    /// </summary>
    public override void Die()
    {
        Alive.SetValue(false);
    }

    private void CreateBehaviorTreePirate()
    {
        /*return new Root(
            new Selector(
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
                            targetPosition = closestPos;
                        })
                    { Label = "Set target position to nearest hunting area" },
                    new Service(0.5f, UpdateDistanceToTargetPosition,
                        new Action((bool shouldCancel) =>
                            {
                                UpdateSafetyStatus();
                                if (blackboard.Get<int>("fearLevel") != 0)
                                {
                                    shouldCancel = true;
                                }
                                if (!shouldCancel)
                                {
                                    MoveTowards(blackboard.Get<Vector3>("targetPos"));
                                    if (blackboard.Get<float>("targetDistance") < 5f)
                                    {
                                        //return Action.Result.SUCCESS;
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
                ),
                new Sequence(
                    new Service(0.5f, UpdateSafetyStatus,
                        new Action((bool shouldCancel) =>
                            {
                                /*if (blackboard.Get<int>("fearLevel") == 0)
                                {
                                    shouldCancel = true;
                                }*//*
                                if (!shouldCancel)
                                {
                                    Vector3 scaryPosition = blackboard.Get<Vector3>("scaryPosition");
                                    Debug.DrawLine(this.transform.position, scaryPosition, Color.red, 1f);
                                    MoveTowards((blackboard.Get<Vector3>("fleeDirection") * 1000f) + scaryPosition);
                                    Vector3 dist = scaryPosition - transform.position;
                                    if (dist.magnitude > 50f)
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
                            { Label = "Run away from scary" }
                    )
                )
            )
        );*/
    }

    private void CreateBehaviourTreeDumbDelivery()
    {
        /*
        return new Root(
            new Sequence(
                new Wait(1f),
                new Action(() =>
                    {
                        targetPosition = blackboard.Get<Planet>("deliveryPlanet").transform.position;
                    })
                { Label = "Set target position to go to drop off planet" },
                new Service(0.5f, UpdateDistanceToTargetPosition,
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
                new Service(0.5f, UpdateDistanceToTargetPosition,
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
                new Service(0.5f, UpdateDistanceToTargetPosition,
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
        );*/
    }

    /// <summary>
    /// Returns a list of mid points between planets that are close to each other.
    /// </summary>
    /// <returns></returns>
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

    /*
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
    */
    private void BlackboardSetNearestPlanet()
    {
        //blackboard.Set("homePlanet", GetNearestPlanet());
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

    Vector3 targetPosition = Vector3.zero;
    private void UpdateDistanceToTargetPosition()
    {
        //BehaviorTree.Blackboard["targetPos"] = targetPosition;
        //BehaviorTree.Blackboard["targetDistance"] = Vector3.Distance(this.transform.position, targetPosition);
    }

    List<Spaceship> GetHostileShipsInRange()
    {
        Faction myFaction = shipScript.Value.Faction;
        List<Spaceship> resultsList = new List<Spaceship>();

        foreach (Spaceship f in shipScript.Value.GetShipsInSensorRange())
        {
            if (f.Faction.HostileWith(myFaction))
            {
                resultsList.Add(f);
            }
        }

        return resultsList;
    }

    private void UpdateSafetyStatus()
    {
        int fear_level = 0;
        List<Spaceship> scaryList = new List<Spaceship>();
        var list = GetHostileShipsInRange();
        var list2 = shipScript.Value.GetShipsInSensorRange();
        foreach (Spaceship f in GetHostileShipsInRange())
        {
            // add to fear level only positive values, since weak ships shouldn't make you fight a carrier
            fear_level += Mathf.Clamp(f.GetScaryness(shipScript.Value), 0, int.MaxValue);
            scaryList.Add(f);
        }

        Vector3 averageScaryPosition = Vector3.zero;
        foreach (Spaceship f in scaryList)
        {
            averageScaryPosition += f.transform.position;
        }
        averageScaryPosition /= scaryList.Count;

        Vector3 fleeDirection = ((averageScaryPosition - transform.position) *-1).normalized;

        if (scaryList.Count > 0)
        {
            //BehaviorTree.Blackboard["fleeDirection"] = fleeDirection;
            //BehaviorTree.Blackboard["scaryPosition"] = averageScaryPosition;
        }
        //BehaviorTree.Blackboard["fearLevel"] = fear_level;
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

    /*
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
            AsteroidField finalTarget = blackboard.Get<AsteroidField>("bestMiningField");
            if (!targets.Contains(finalTarget))
            {
                Debug.Log("THIS SHOULDN'T HAPPEN!");
            }

            minedAmount = shipScript.CargoHold.Credit(miningTarget, finalTarget.CargoHold, mineAmount);
        }

        return minedAmount;
    }
    
    private void DropOffMinedResources()
    {
        List<Planet> planets = shipScript.Value.GetInInteractionRange<Planet>();
        if (planets.Contains(TargetPlanet.Value))
        {
            List<string> miningTargetsList = blackboard.Get<List<string>>("miningTargetsList");
            foreach (string resource in miningTargetsList)
            {
                homePlanet.CargoHold.Credit(resource, shipScript.CargoHold, shipScript.CargoHold.GetAmountInHold(resource), true);
            }
        }
    }*/

    private void DropOffResource(MarketOrder type)
    {
        DropOffResource(type.item.Name);
        type.Succeed();
        //throw new NotImplementedException();
    }

    private int DropOffResource(String type)
    {
        return GetNearestPlanet().GetCargoHold.Credit(type, shipScript.Value.GetCargoHold, shipScript.Value.GetCargoHold.GetAmountInHold(type), true);
    }

    /// <summary>
    /// Returns whether checkPoint is on the left side of the line defined by pos1 and pos2.
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    /// <param name="checkPoint"></param>
    /// <returns></returns>
    private bool isLeft(Vector3 pos1, Vector3 pos2, Vector3 checkPoint)
    {
        return ((pos2.x - pos1.x) * (checkPoint.z - pos1.z) - (pos2.z - pos1.z) * (checkPoint.x - pos1.x)) > 0;
    }

    public void NotifyShip(Spaceship contact)
    {
        /*if (blackboard != null)
        {
            blackboard["sensor_contact"] = true;
        }*/
    }
}
