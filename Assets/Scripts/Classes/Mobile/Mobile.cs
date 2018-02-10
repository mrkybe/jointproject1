using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI_Missions;

[SelectionBase]
public class Mobile : MonoBehaviour
{
    /* Purpose: Define the base class for all entities that will be moving
     *          every update.
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

    public Rigidbody rigidbody;
    // Use this for initialization
    protected void Start()
    {
        inTime = true;
        rigidbody = GetComponent<Rigidbody>();
    }

    protected void Update()
    {
        if (inTime)
        {
            CalculateGravityVector();
        }
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
}
