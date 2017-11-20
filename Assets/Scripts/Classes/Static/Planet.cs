using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.WorldSingleton;
using ShipInternals;

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
    private CargoHold availableCargoItems;

    void Start ()
    {
        WorkerShips = new List<GameObject>();

        listOfPlanetObjects.Add(this);

        TimeToSpawn = gameObject.AddComponent<Timer>();
        TimeToSpawn.SetTimer(1);
        TimeToSpawn.Loop(true);
        

        myStorage = new CargoHold(50000);
        availableCargoItems = new CargoHold(50000);

        SetupBuildings();
		SetupMarket ();
		behaviorTree;
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
        int startingCount = random.Next(8) + 3;
        for (int i = 0; i < startingCount; i++)
        {
            myBuildings.Add(Building.BasicEnviroments[random.Next(4)]());
        }
        myBuildings.Sort((a,b) => string.CompareOrdinal(a.Name, b.Name));
        TickBuildings(random.Next(25) + 25);
    }

	public void SetupMarket()
	{


	}

    public void SpawnMiningShip()
    {
        var ship = (GameObject)Instantiate(Resources.Load("Prefabs/AI_ship"), this.transform.position, Quaternion.identity);
        WorkerShips.Add(ship.gameObject);
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
}
