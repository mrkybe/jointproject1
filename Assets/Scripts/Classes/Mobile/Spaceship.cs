using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI_Missions;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.WorldSingleton;
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
    [SerializeField]
    public Faction Faction;
    [SerializeField]
    public int PowerLevel;

    private float targetSpeed;
    private float throttle_input;
    private float oldThrottle_input;
    private CargoHold myStorage;
    private SensorArray mySensorArray;
    // Use this for initialization

    [SerializeField]
    public List<GameObject> inSensorRange = new List<GameObject>();

    void Awake()
    {
        if (pilot == null)
        {
            SetPilot(desired_AI_Type);
        }

        pilot.SensorArray = mySensorArray;

        engineRunSpeed = 0;
        targetSpeed = -999;
        throttle_input = 0;
        oldThrottle_input = 0;
        if (isAI)
        {
            targetSpeed = 0;
        }
        myStorage = new CargoHold(100);

        mySensorArray = new SensorArray(gameObject);
    }

    new void Start ()
    {
        base.Start();
	}

	// Update is called once per frame
	new void Update ()
	{
        if (inTime && pilot)
        {
            base.Update();
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
        inSensorRange.Add(other.gameObject.transform.root.gameObject);
    }

    public void CleanSensorList()
    {
        inSensorRange.RemoveAll(x => x == null);
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Unboop");
        inSensorRange.Remove(other.gameObject.transform.root.gameObject);
    }

    public void SetModel(int number)
    {
        MeshFilter mf = GetComponentInChildren<MeshFilter>();
        MeshRenderer mr = GetComponentInChildren<MeshRenderer>();
    }

    public List<Spaceship> GetShipsInRange()
    {
        List<Spaceship> targets = new List<Spaceship>();
        CleanSensorList();
        for (int i = 0; i < inSensorRange.Count; i++)
        {
            if (inSensorRange[i] != null)
            {
                Spaceship target = inSensorRange[i].GetComponent<Spaceship>();
                if (target != null && Vector3.Distance(transform.position, inSensorRange[i].transform.root.position) < 8)
                {
                    targets.Add(target);
                }
            }
        }
        return targets;
    }

    public List<Static> GetStaticInRange()
    {
        List<Static> targets = new List<Static>();
        CleanSensorList();
        for (int i = 0; i < inSensorRange.Count; i++)
        {
            if (inSensorRange[i] != null)
            {
                Static target = inSensorRange[i].GetComponent<Static>();
                if (target != null && Vector3.Distance(transform.position, inSensorRange[i].transform.root.position) < 8)
                {
                    targets.Add(target);
                }
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

    public int GetScaryness(Spaceship shipScript)
    {
        return PowerLevel - shipScript.PowerLevel;
    }
}