using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.WorldSingleton;
using ShipInternals;

/// <summary>
/// A Base that belongs to a faction.  Sends out ships and trades resources.
/// </summary>
public partial class Planet : Static
{
    [SerializeField]
    private List<GameObject> WorkerShips;

    [SerializeField]
    public Faction Faction;

    [SerializeField]
    public string MyName;

    [SerializeField]
    private Timer TimeToSpawn;

    [SerializeField]
    public float Mass = 1;

    [SerializeField]
    public float Radius = 1;

    [SerializeField]
    int MaxFriends;

    [SerializeField]
    private List<Building> myBuildings = new List<Building>();

    [SerializeField]
    public bool hasGravity;

    [SerializeField]
    public static List<Planet> listOfPlanetObjects = new List<Planet>();

    [SerializeField]
    private CargoHold myStorage;

    [SerializeField]
    private CargoHold reservedStorage;

    private Faction faction;

    void Awake()
    {
        WorkerShips = new List<GameObject>();

        listOfPlanetObjects.Add(this);

        TimeToSpawn = gameObject.AddComponent<Timer>();
        TimeToSpawn.SetTimer(1);
        TimeToSpawn.Loop(true);
        

        myStorage = new CargoHold(50000);
        reservedStorage = new CargoHold(50000);

        SetupBuildings();
		SetupMarket();
        PlanetBTSetup();
        //behaviorTree; // Was causing compile errors
    }


    public void SetFaction(Faction f)
    {
        if (Faction != null)
        {
            Faction.Unown(this);
        }

        Faction = f;
        f.Own(this);

        MeshRenderer mr = transform.GetChild(0).GetComponent<MeshRenderer>();
        mr.material.color = f.ColorPrimary;
    }

    public void SetupBuildings()
    {
        System.Random random = new System.Random(GetInstanceID());
        int enviromentalStartingCount = random.Next(2) + 3;
        int industrialStartingCount = random.Next(2) + 1;
        // Add enviromental buildings
        for (int i = 0; i < enviromentalStartingCount; i++)
        {
            myBuildings.Add(Building.BasicEnviroments[random.Next(Building.BasicEnviroments.Length)]());
        }
        // Add industrial buildings
        for (int i = 0; i < industrialStartingCount; i++)
        {
            myBuildings.Add(Building.BasicIndustry[random.Next(Building.BasicIndustry.Length)]());
        }
        myBuildings.Sort((a,b) => string.CompareOrdinal(a.Name, b.Name));
        TickBuildings(random.Next(25) + 25);
    }

	public void SetupMarket()
	{


	}

    public void RandomizeSize()
    {
        System.Random random = new System.Random(GetInstanceID());

        /*Radius = (float)(random.NextDouble() * 13) + 2f;
        Mass = (float)(4 * Math.PI * Math.Pow(Radius / 2, 3));*/
        Radius = (float)(random.NextDouble() * 1) + 2f;
        Mass = (float)(4 * Math.PI * Math.Pow(Radius / 2, 3));

        transform.localScale += (new Vector3(Radius * 2f, Radius * 2f, Radius * 2f) - transform.localScale);
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * -1f);
	}

    void FixedUpdate()
    {
        if (inTime)
        {
            
        }
    }

    private void TickBuildings(int multiplier = 1)
    {
        for (int i = 0; i < multiplier; i++)
        {
            foreach (var building in myBuildings)
            {
                building.Tick(myStorage);
            }
        }
    }

    public CargoHold GetCargoHold
    {
        get { return myStorage; }
    }

    public CargoHold GetReserveCargoHold
    {
        get { return reservedStorage; }
    }

    public string BuildingsToString(string seperator = "\n")
    {
        string BuildingsNamed = "";
        foreach (var building in myBuildings)
        {
            BuildingsNamed += building.Name + seperator;
        }
        return BuildingsNamed;
    }

    public void SetName(string val)
    {
        MyName = val;
    }


    /// <summary>
    /// Called by Overseer.cs.  Not every planet has its Tick called at the same time.  You have been warned.
    /// </summary>
    public void Tick()
    {
        TickBuildings();
    }

    public class PlanetComparer : IComparer<Planet>
    {
        private Planet origin;

        public PlanetComparer(Planet origin)
        {
            this.origin = origin;
        }

        public int CompareClosest(Planet x, Planet y)
        {
            Vector3 xPos = x.transform.position;
            Vector3 yPos = y.transform.position;
            Vector3 originPos = origin.transform.position;
            float xDist = Vector3.Distance(originPos, xPos);
            float yDist = Vector3.Distance(originPos, yPos);
            if (xDist < yDist)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        public int Compare(Planet x, Planet y)
        {
            return CompareClosest(x, y);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    protected new void OnDestroy()
    {
        base.OnDestroy();
        listOfPlanetObjects.Remove(this);
    }

}
