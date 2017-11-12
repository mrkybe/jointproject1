using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShipInternals;

public class Planet : Static
{
    [SerializeField]
    private List<GameObject> WorkerShips;

    [SerializeField]
    private Timer TimeToSpawn;

    [SerializeField]
    public float Mass = 1;

    [SerializeField]
    public float Radius = 1;

    [SerializeField]
    int MaxFriends;

    [SerializeField]
    private CargoHold myStorage;

    [SerializeField]
    private List<Building> myBuildings = new List<Building>();

    [SerializeField]
    public bool hasGravity;

    [SerializeField]
    public static List<Planet> listOfPlanetObjects = new List<Planet>();

	[SerializeField]
	public static List<CargoHold> availableStocks = new List<CargoHold> ();

    void Start ()
    {
        WorkerShips = new List<GameObject>();

        listOfPlanetObjects.Add(this);

        TimeToSpawn = gameObject.AddComponent<Timer>();
        TimeToSpawn.SetTimer(1);
        TimeToSpawn.Loop(true);
        
        myStorage = new CargoHold(50000);
        //myStorage.printHold();

        SetupBuildings();
		SetupMarket ();
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
        Debug.Log(this.name + " | " + BuildingsToString(", "));
        TickBuildings();
    }

	public void SetupMarket()
	{
		availableStocks.Add (GetCargoHold);



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

    private void TickBuildings()
    {
        foreach (var building in myBuildings)
        {
            building.Tick(myStorage);
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
}
