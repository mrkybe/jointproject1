using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI_Missions;
using NPBehave;
using ShipInternals;

public abstract class PilotInterface : MonoBehaviour
{
    protected SensorArray mySensorArray;
    protected Planet homePlanet;
    protected Vector2 control_stickDirection;
    protected float targetSpeed;
    protected Root behaviorTree;

    public SensorArray SensorArray
    {
        get { return mySensorArray; }
        set { mySensorArray = value; }
    }

    // Use this for initialization
    protected void Start()
    {
        control_stickDirection = new Vector2();
        targetSpeed = 0;
	}

    protected void Update()
    {

	}

    public Vector3 Direction
    {
        get { return control_stickDirection.normalized; }
    }

    public float Throttle
    {
        get { return Mathf.Clamp(control_stickDirection.y, -1f, 1f); }
    }

    public float Turning
    {
        get { return Mathf.Clamp(control_stickDirection.x, -1f, 1f); }
    }

    public float TargetSpeed
    {
        get
        {
            //Debug.Log("Tried to get targetSpeed, gonna tell him " + targetSpeed);
            return targetSpeed;
        }
    }

    public abstract void Die();
}
