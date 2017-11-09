using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI_Missions;
using ShipInternals;

public class Spaceship : Mobile
{
    private float engineRunSpeed;
    [SerializeField]
    private float engineAcceleration;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float turningSpeed;
    [SerializeField]
    private float manuverability;
    [SerializeField]
    private AI_Type desired_AI_Type;
    private float targetSpeed;
    private float throttle_input;
    private float oldThrottle_input;
    private CargoHold myStorage;
    private SensorArray mySensorArray;
    // Use this for initialization
    new void Start ()
    {
        base.Start();
        if(pilot == null)
        {
            SetPilot(desired_AI_Type);
        }

        pilot.SensorArray = mySensorArray;

        engineRunSpeed = 0;
        targetSpeed = -999;
        throttle_input = 0;
        oldThrottle_input = 0;
        if ( isAI )
        {
            targetSpeed = 0;
        }
        myStorage = new CargoHold(50);
        myStorage.AddHoldType("Gold");

        mySensorArray = new SensorArray(gameObject);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            myStorage.PrintHold();
        }
    }
	
	// Update is called once per frame
	new void FixedUpdate ()
    {
        if (inTime && pilot)
        {
            base.FixedUpdate();
            direction = transform.forward;
            velocity = engineRunSpeed;

            if (pilot.Throttle > 0)
            {
                engineRunSpeed += engineAcceleration * pilot.Throttle;
            }
            else if (pilot.Throttle < 0 && engineRunSpeed > 0)
            {
                engineRunSpeed += engineAcceleration * pilot.Throttle;
            }
            else if (engineRunSpeed < 0)
            {
                engineRunSpeed = 0;
            }
            if (targetSpeed != -999)
            {
                targetSpeed = pilot.TargetSpeed;
                //Debug.Log(targetSpeed);
                engineRunSpeed += engineAcceleration * Mathf.Clamp((targetSpeed - engineRunSpeed) * 10000, -1f, 1f);
                //Debug.Log(engineRunSpeed);
                if (Mathf.Abs(engineRunSpeed) < engineAcceleration )
                {
                    engineRunSpeed = 0;
                }
            }

            engineRunSpeed = Mathf.Clamp(maxSpeed, -(maxSpeed / 15), engineRunSpeed);
            throttle_input = engineAcceleration * Mathf.Clamp((targetSpeed - engineRunSpeed), -1f, 1f);
            transform.Rotate(Vector3.up * getVelocityPercentage() * (pilot.Turning * turningSpeed));
            oldThrottle_input = throttle_input;
        }
	}

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Boop");
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Unboop");
    }
    
    public List<Static> getAvailableTargets()
    {
        List<Static> targets = new List<Static>();
        for (int i = 0; i < Static.listOfStaticObjects.Count; i++)
        {
            if (Vector3.Distance(transform.position, Static.listOfStaticObjects[i].transform.position) < 8)
            {
                targets.Add(Static.listOfStaticObjects[i]);
            }
        }
        return targets;
    }

    private float getVelocityPercentage()
    {
        return (manuverability + Mathf.Abs(engineRunSpeed - maxSpeed) * (1-manuverability));
    }

    public float EngineRunSpeed
    {
        get { return engineRunSpeed; }
        set { engineRunSpeed = value; }
    }
    public float EngineAcceleration
    {
        get { return engineAcceleration; }
        set { engineAcceleration = value; }
    }
    public float MaxSpeed
    {
        get { return maxSpeed; }
        set { maxSpeed = value; }
    }
    public float TurningSpeed
    {
        get { return turningSpeed; }
        set { turningSpeed = value; }
    }

    public float Manuverability
    {
        get { return manuverability; }
        set { manuverability = value; }
    }

    public float TargetSpeed
    {
        get { return targetSpeed; }
        set { targetSpeed = value; }
    }

    public float ThrottleInput
    {
        get { return throttle_input; }
        set { throttle_input = value; }
    }

    public CargoHold GetCargoHold
    {
        get { return myStorage; }
    }
}