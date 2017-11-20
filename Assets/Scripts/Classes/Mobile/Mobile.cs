using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI_Missions;

public class Mobile : MonoBehaviour
{
    /* Written by Ruslan Kaybyshev, Late 2013
     * Purpose: Define the base class for all entities that will be moving
     *          every fixed update.  This class will be derived to create
     *          ships, asteroids, bullets, missles.  Lasers are instant
     *          (probably) and therefore are not derived from moveable. (probably)
     */
    protected Vector3 direction;
    protected float velocity;
    [SerializeField]
    protected bool isAI;
    [SerializeField]
    protected bool isAffectedByGravity;
    protected PilotInterface pilot;
    static public bool inTime;
    private Vector3 gravityVector;
    // Use this for initialization
    protected void Start()
    {
        inTime = true;
    }

    public void SetPilot(AI_Type wanted)
    {
        switch (wanted)
        {
            case AI_Type.GATHER:
                pilot = gameObject.AddComponent<AI_Gather>();
                isAI = true;
                break;
            case AI_Type.PATROL:
                pilot = gameObject.AddComponent<AI_Patrol>();
                isAI = true;
                break;
            case AI_Type.PLAYER:
                pilot = gameObject.AddComponent<PlayerPilot>();
                isAI = false;
                break;
            default:
                Debug.Log("NO PILOT SET!!!");
                break;
        }
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (inTime)
        {
            CalculateGravityVector();
            Move();
        }
    }

    protected void Update()
    {
        
	}

    private void CalculateGravityVector()
    {
        //Debug.Log("GRAVITY VECTOR " + Planet.listOfPlanetObjects.Count);
        gravityVector = Vector3.zero;
        
        foreach (var x in Planet.listOfPlanetObjects)
        {
            //List<Vector3> planetPositions = new List<Vector3>();
            //List<double> planetMasses = new List<double>();

            if (x.hasGravity)
            {
                Vector3 offset = x.transform.position - transform.position;
                double g = x.Mass / offset.sqrMagnitude;
                if (offset.magnitude < (x.Radius + 1))
                {
                    g = 0;
                }
                Vector3 norm = offset.normalized;
                norm.Scale(new Vector3((float)g, 0, (float)g));
                gravityVector = gravityVector + norm;
                //Debug.Log(g);
                //planetPositions.Add(offset);
                //planetMasses.Add(x.MassKilotons);
            }
        }
    }

    private void Move()
    {
        Vector3 engineVel = (velocity * direction.normalized);
        Vector3 gravityVel = Vector3.zero;
        if (isAffectedByGravity)
        {
            gravityVel = gravityVector / Mathf.Pow(10,30);
            Debug.DrawLine(this.transform.position, this.transform.position + gravityVel);
        }
        Vector3 finalVel = (gravityVel + engineVel) * Time.deltaTime;

        transform.position = transform.position + finalVel;
    }
}
