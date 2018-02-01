using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI_Missions;
using Assets.Scripts.Classes.Helper;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.WorldSingleton;
using ShipInternals;

/// <summary>
/// The physical Spaceship.  Keeps track of 'physical' information, moves it every update.  Requires a Pilot, human or AI.
/// </summary>
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
    [SerializeField]
    public int HullHealth;
    [SerializeField]
    public float InteractionRange;

    private float targetSpeed;
    private float throttle_input;
    private float oldThrottle_input;
    private CargoHold myStorage;
    private SensorArray mySensorArray;
    private ModelSwitcher myModelSwitcher;
    // Use this for initialization

    [SerializeField]
    public List<GameObject> inSensorRange = new List<GameObject>();

    private int modelChoice = 0;
    void Awake()
    {
        pilot = GetComponent<AI_Patrol>();
        if (pilot == null)
        {
            pilot = GetComponent<PlayerPilot>();
        }

        pilot.SensorArray = mySensorArray;

        engineRunSpeed = 0;
        targetSpeed = -999;
        throttle_input = 0;
        oldThrottle_input = 0;
        PowerLevel = 10;
        HullHealth = 100;
        InteractionRange = 8f;

        if (isAI)
        {
            targetSpeed = 0;
        }
        myStorage = new CargoHold(100);

        mySensorArray = new SensorArray(gameObject);
        modelChoice = (int)(Random.value * 11);
    }

    new void Start ()
    {
        base.Start();
        myModelSwitcher = GetComponentInChildren<ModelSwitcher>();
        myModelSwitcher.SetModel(modelChoice);
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
        // New entity in sensor range.
        inSensorRange.Add(other.gameObject.transform.root.gameObject);
        if (pilot.GetType() == typeof(AI_Patrol))
        {
            ((AI_Patrol)pilot).Notify(other.gameObject.transform.root.gameObject);
        }
    }

    public void CleanSensorList()
    {
        inSensorRange.RemoveAll(x => x == null);
    }

    public void OnTriggerExit(Collider other)
    {
        // Entity leaves sensor range.
        inSensorRange.Remove(other.gameObject.transform.root.gameObject);
    }

    /// <summary>
    /// Returns a list of Spaceships in sensor range.
    /// </summary>
    /// <returns></returns>
    public List<Spaceship> GetShipsInSensorRange()
    {
        List<Spaceship> targets = new List<Spaceship>();
        CleanSensorList();
        for (int i = 0; i < inSensorRange.Count; i++)
        {
            Spaceship target = inSensorRange[i].GetComponent<Spaceship>();
            if (target != null)
            {
                targets.Add(target);
            }
        }
        return targets;
    }

    /// <summary>
    /// Returns a list of Static entities in sensor range.
    /// </summary>
    /// <returns></returns>
    public List<Static> GetStaticInSensorRange()
    {
        List<Static> targets = new List<Static>();
        CleanSensorList();
        for (int i = 0; i < inSensorRange.Count; i++)
        {
            Static target = inSensorRange[i].GetComponent<Static>();
            if (target != null)
            {
                targets.Add(target);
            }
        }
        return targets;
    }

    /// <summary>
    /// Returns a list of specified entities in sensor range.
    /// </summary>
    /// <returns></returns>
    public List<T> GetInSensorRange<T>()
    {
        List<T> targets = new List<T>();
        CleanSensorList();
        for (int i = 0; i < inSensorRange.Count; i++)
        {
            T target = inSensorRange[i].GetComponent<T>();
            if (target != null)
            {
                targets.Add(target);
            }
        }
        return targets;
    }

    /// <summary>
    /// Returns a list of Spaceships in interaction range.
    /// </summary>
    /// <returns></returns>
    public List<Spaceship> GetShipsInInteractionRange()
    {
        List<Spaceship> targets = new List<Spaceship>();
        CleanSensorList();
        for (int i = 0; i < inSensorRange.Count; i++)
        {
            if (inSensorRange[i] != null)
            {
                Spaceship target = inSensorRange[i].GetComponent<Spaceship>();
                if (target != null && Vector3.Distance(transform.position, inSensorRange[i].transform.root.position) < InteractionRange)
                {
                    targets.Add(target);
                }
            }
        }
        return targets;
    }

    public List<Static> GetStaticInInteractionRange()
    {
        List<Static> targets = new List<Static>();
        CleanSensorList();
        for (int i = 0; i < inSensorRange.Count; i++)
        {
            if (inSensorRange[i] != null)
            {
                Static target = inSensorRange[i].GetComponent<Static>();
                if (target != null && Vector3.Distance(transform.position, inSensorRange[i].transform.root.position) < InteractionRange)
                {
                    targets.Add(target);
                }
            }
        }
        return targets;
    }

    /// <summary>
    /// Returns a list of specified entities in interaction range.
    /// </summary>
    /// <returns></returns>
    public List<T> GetInInteractionRange<T>()
    {
        List<T> targets = new List<T>();
        CleanSensorList();
        for (int i = 0; i < inSensorRange.Count; i++)
        {
            if (inSensorRange[i] != null)
            {
                T target = inSensorRange[i].GetComponent<T>();
                if (target != null && Vector3.Distance(transform.position, inSensorRange[i].transform.root.position) < InteractionRange)
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

    public PilotInterface GetPilot
    {
        get { return pilot; }
    }

    /// <summary>
    /// Returns how scary another ship is compared to mine.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int GetScaryness(Spaceship other)
    {
        return PowerLevel - other.PowerLevel;
    }

    public void TakeDamage(int i)
    {
        HullHealth -= i;
        if (HullHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        myModelSwitcher.BecomeGraveyard();
        GetPilot.Die();
    }
}