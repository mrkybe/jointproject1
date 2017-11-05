using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShipInternals;

public class Planet : Static
{
    int Population;
    int RawMaterial;
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
    string Faction;
    private CargoHold myStorage;
    private List<Building> myBuildings = new List<Building>();

    [SerializeField]
    public bool hasGravity;

    [SerializeField]
    private List<GameObject> AsteroidFields = new List<GameObject>();

    [SerializeField]
    public static List<Planet> listOfPlanetObjects = new List<Planet>();
    // Use this for initialization

    void Start ()
    {
        WorkerShips = new List<GameObject>();

        listOfPlanetObjects.Add(this);

        TimeToSpawn = gameObject.AddComponent<Timer>();
        TimeToSpawn.SetTimer(1);
        TimeToSpawn.Loop(true);
        
        myStorage = new CargoHold(50000);
        myStorage.AddHoldType("Rock");
        myStorage.AddHoldType("Gold");
        myStorage.AddHoldType("Food");
        myStorage.AddToHold("Rock", 3000);
        //myStorage.printHold();

        SetupBuildings();
    }

    public void SetupBuildings()
    {
        System.Random random = new System.Random(GetInstanceID());
        myBuildings.Add(Building.BasicEnviroments[random.Next(4)]());
        myBuildings.Add(Building.BasicEnviroments[random.Next(4)]());
        myBuildings.Add(Building.BasicEnviroments[random.Next(4)]());
        myBuildings.Add(Building.BasicEnviroments[random.Next(4)]());

        string BuildingsNamed = "";
        foreach (var building in myBuildings)
        {
            BuildingsNamed += building.Name + ", ";
        }
        Debug.Log(this.name + " | " + BuildingsNamed);
    }

    public void SpawnMiningShip()
    {
        var ship = Instantiate(Resources.Load("Prefabs/AI_ship"), this.transform.position, Quaternion.identity);
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

    public CargoHold GetCargoHold
    {
        get { return myStorage; }
    }
}
