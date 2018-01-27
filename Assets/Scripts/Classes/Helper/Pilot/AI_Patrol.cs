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

/* AI_Patrol is the AI for the ships/fleets on the Overmap.
 * It contains all of the atomic AI methods that are used by the AI behavior tree.
 * It provides public functions for Factions/Planets to give orders through.
 * These orders are fulfilled by replacing the current BehaviorTree with a new one.
 *      
 */

public class AI_Patrol : PilotInterface
{
    private Blackboard blackboard;
    private Spaceship shipScript;
    private Debugger debugger = null;
    private float interactionDistance = 5f;

    // Use this for initialization
    public void Awake()
    {
        shipScript = transform.GetComponent<Spaceship>();

#if UNITY_EDITOR
        if (debugger == null)
        {
            debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        }
#endif
    }

    public new void Start()
    {
        base.Start();
    }

    public void OnDestroy()
    {
        if (behaviorTree != null)
        {
            behaviorTree.Stop();
        }
    }

    /// <summary>
    /// Returns the Shipscript that I am the pilot of.
    /// </summary>
    /// <returns></returns>
    public Spaceship GetShip()
    {
        return shipScript;
    }

    /// <summary>
    /// Sets the Behavior Tree the one for mining.
    /// </summary>
    /// <param name="miningTargets">The kind of resources to mine.</param>
    public void StartMining(List<string> miningTargets, Planet homePlanet)
    {
        behaviorTree = CreateBehaviourTreeDumbMining();

        blackboard = behaviorTree.Blackboard;

        InitializeDefaultBlackboard();
        blackboard["miningTargetsList"] = miningTargets;
        
        blackboard["homePlanet"] = homePlanet;
        this.homePlanet = homePlanet;

#if UNITY_EDITOR
        if (debugger == null)
        {
            debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        }
        debugger.BehaviorTree = behaviorTree;
#endif
        behaviorTree.Start();
    }

    /// <summary>
    /// Sets the Behavior Tree the one for delivering an order.
    /// </summary>
    /// <param name="order">The order that the ship is responsible for completing.</param>
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
            InitializeDefaultBlackboard();
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

    /// <summary>
    /// Sets the Behavior Tree to the one for piracy.
    /// </summary>
    public void StartPirate()
    {
        if (behaviorTree != null)
        {
            behaviorTree.Stop();
        }

        behaviorTree = CreateBehaviorTreePirate();
        blackboard = behaviorTree.Blackboard;
        InitializeDefaultBlackboard();

        shipScript.Faction = Overseer.Main.GetFaction("Pirates");
        
        UpdateSafetyStatus();

#if UNITY_EDITOR
        if (debugger == null)
        {
            debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        }
        debugger.BehaviorTree = behaviorTree;
#endif
        behaviorTree.Start();
    }

    /// <summary>
    /// Setup the things that all behavior trees will share.
    /// </summary>
    private void InitializeDefaultBlackboard()
    {
        blackboard["dead"] = false;
    }

    /// <summary>
    /// This kills the Pilot.  Sets a Flag in the Blackboard for being dead,
    /// Behavior Trees must clean up after themselves and go into their dead state.
    /// </summary>
    public override void Die()
    {
        if (blackboard != null)
        {
            blackboard["dead"] = true;
        }
    }

    private Root CreateBehaviorTreePirate()
    {
        return new Root(
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
                                }*/
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
        );
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
        );
    }

    private Root CreateBehaviourTreeDumbMining()
    {
        return new Root(
            new Sequence(
                new Wait(1f),
                new Action((bool shouldCancel) =>
                            {
                                AsteroidField bestCandidate = FindNearestAsteroidFieldForMining(blackboard.Get<List<string>>("miningTargetsList"));
                                if (bestCandidate != null)
                                {
                                    blackboard["bestMiningField"] = bestCandidate;
                                    targetPosition = bestCandidate.transform.position;
                                    blackboard["targetPos"] = bestCandidate.transform.position;
                                    return Action.Result.SUCCESS;
                                }
                                else
                                {
                                    return Action.Result.FAILED;
                                }
                            })
                { Label = "Find best nearest AsteroidField for mining" },
                new Service(0.5f, UpdateDistanceToTargetPosition,
                    new Action((bool shouldCancel) =>
                                {
                                    if (!shouldCancel)
                                    {
                                        MoveTowards(blackboard.Get<Vector3>("targetPos"));
                                        if (blackboard.Get<float>("targetDistance") < interactionDistance)
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
                    { Label = "Go to target position" }
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
                                    else
                                    {
                                        int mined = Mine(blackboard.Get<List<string>>("miningTargetsList"));
                                        if (mined > 0)
                                        {
                                            return Action.Result.SUCCESS;
                                        }
                                    }
                                    return Action.Result.FAILED; // this should search for minable asteroid fields better
                                })
                    { Label = "Mining" }
                ))),
                new Action(() =>
                            {
                                targetPosition = blackboard.Get<Planet>("homePlanet").transform.position;
                            })
                { Label = "Set target position to go to home planet" },
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
                                DropOffMinedResources();
                            })
                { Label = "Dropping off resources" }
                )
        );
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
            if (field.GetCargoHold.Contains(resource) && field.GetCargoHold.GetAmountInHold(resource) > 0)
            {
                success.Add(resource);
            }
        }

        return success;
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

    Vector3 targetPosition = Vector3.zero;
    private void UpdateDistanceToTargetPosition()
    {
        behaviorTree.Blackboard["targetPos"] = targetPosition;
        behaviorTree.Blackboard["targetDistance"] = Vector3.Distance(this.transform.position, targetPosition);
    }

    List<Spaceship> GetHostileShipsInRange()
    {
        Faction myFaction = shipScript.Faction;
        List<Spaceship> resultsList = new List<Spaceship>();

        foreach (Spaceship f in shipScript.GetShipsInSensorRange())
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
        var list2 = shipScript.GetShipsInSensorRange();
        foreach (Spaceship f in GetHostileShipsInRange())
        {
            // add to fear level only positive values, since weak ships shouldn't make you fight a carrier
            fear_level += Mathf.Clamp(f.GetScaryness(shipScript), 0, int.MaxValue);
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
            behaviorTree.Blackboard["fleeDirection"] = fleeDirection;
            behaviorTree.Blackboard["scaryPosition"] = averageScaryPosition;
        }
        behaviorTree.Blackboard["fearLevel"] = fear_level;
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

            minedAmount = shipScript.GetCargoHold.Credit(miningTarget, finalTarget.GetCargoHold, mineAmount);
        }

        return minedAmount;
    }

    private void DropOffMinedResources()
    {
        List<Planet> planets = shipScript.GetInInteractionRange<Planet>();
        if (planets.Contains(homePlanet))
        {
            List<string> miningTargetsList = blackboard.Get<List<string>>("miningTargetsList");
            foreach (string resource in miningTargetsList)
            {
                homePlanet.GetCargoHold.Credit(resource, shipScript.GetCargoHold, shipScript.GetCargoHold.GetAmountInHold(resource), true);
            }
        }
    }

    private void DropOffResource(MarketOrder type)
    {
        DropOffResource(type.item.Name);
        type.Succeed();
        //throw new NotImplementedException();
    }

    private int DropOffResource(String type)
    {
        return GetNearestPlanet().GetCargoHold.Credit(type, shipScript.GetCargoHold, shipScript.GetCargoHold.GetAmountInHold(type), true);
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

    public void Notify(GameObject contact)
    {
        blackboard["sensor_contact"] = true;
    }
}
