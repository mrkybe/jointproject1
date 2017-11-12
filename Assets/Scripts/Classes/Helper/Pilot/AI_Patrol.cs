using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI_Missions;
using NPBehave;
using ShipInternals;
using Action = NPBehave.Action;

public class AI_Patrol : PilotInterface
{
    private Blackboard blackboard;
    private Spaceship shipScript;

    // Use this for initialization
    void Start ()
    {
        base.Start();
        shipScript = transform.GetComponent<Spaceship>();
        ai_type = AI_Type.PATROL;
        behaviorTree = CreateBehaviourTreeDumbMining();
        blackboard = behaviorTree.Blackboard;


        // temporarily use this as the home base until we have a better system
        FindNearestPlanet();
        blackboard["miningTarget"] = "Gold";

        // attach the debugger component if executed in editor (helps to debug in the inspector) 
#if UNITY_EDITOR
        Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        debugger.BehaviorTree = behaviorTree;
#endif
        behaviorTree.Start();
    }

    private Root CreateBehaviourTreeDumbMining()
    {

        // we always need a root node
        return new Root(
            new Sequence(
                    new Wait(1f),
                    new Action(() =>
                               {
                                   FindNearestAsteroidField();
                                   targetPosition = blackboard.Get<AsteroidField>("nearestAsteroidField").transform.position;
                               })
                    { Label = "Find Nearest Asteroid Field"},
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
                        { Label = "Mining"}
                    ))),
                    new Action(() =>
                               {
                                   targetPosition = blackboard.Get<Planet>("homePlanet").transform.position;
                               })
                    {Label = "Set target position to go home"},
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
        if(!found)
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
    
    private void FindNearestPlanet()
    {
        float nearestDistance = float.MaxValue;
        foreach (var one in Planet.listOfPlanetObjects)
        {
            if ((transform.position - one.transform.position).magnitude < nearestDistance)
            {
                blackboard.Set("homePlanet", one);
                nearestDistance = (transform.position - one.transform.position).magnitude;
            }
        }
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
            List<Static> targets = shipScript.getAvailableTargets();
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
        return blackboard.Get<Planet>("homePlanet").GetComponent<Planet>().GetCargoHold.Credit(type, shipScript.GetCargoHold, shipScript.GetCargoHold.GetAmountInHold(type));
    }

    public bool isLeft(Vector3 pos1, Vector3 pos2, Vector3 checkPoint)
    {
        return ((pos2.x - pos1.x) * (checkPoint.z - pos1.z) - (pos2.z - pos1.z) * (checkPoint.x - pos1.x)) > 0;
    }
}
