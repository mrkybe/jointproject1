using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShipInternals;
using Object = UnityEngine.Object;

public class Planet : Static
{
    int Population;
    int RawMaterial;
    [SerializeField]
    private List<Object> WorkerShips;


    [SerializeField]
    private Timer TimeToSpawn;

    [SerializeField]
    public double MassKilotons = 3.402*(Mathf.Pow(10,31));

    [SerializeField]
    int MaxFriends;
    string Faction;
    private CargoHold myStorage;

    [SerializeField]
    public bool hasGravity;

    [SerializeField]
    GameObject workership1;

    [SerializeField]
    private List<GameObject> AsteroidFields = new List<GameObject>();

    [SerializeField]
    public static List<Planet> listOfPlanetObjects = new List<Planet>();
    // Use this for initialization

    private float mul;

    void Start ()
    {
        //Object ship = Instantiate(workership1, transform.position + new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10)), Quaternion.identity);
        //WorkerShips.Add(ship);
        listOfPlanetObjects.Add(this);
        MaxFriends = 3;
        System.Random random = new System.Random(GetInstanceID());
        mul = (float) (Math.Sqrt(random.NextDouble()) * 15) - transform.localScale.magnitude;

        TimeToSpawn = gameObject.AddComponent<Timer>();
        TimeToSpawn.SetTimer(1);
        TimeToSpawn.Loop(true);
        transform.localScale += new Vector3(mul, mul, mul);
        //this.GetComponent<Transform>().localScale.Scale(new Vector3(mul, mul, mul));
        myStorage = new CargoHold(5000);
        myStorage.addHoldType("Rock");
        myStorage.addHoldType("Gold");
        myStorage.addHoldType("Food");
        myStorage.addToHold("Rock", 3000);
        //myStorage.printHold();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (TimeToSpawn.Done)
        {
            //Debug.Log("TICK TOCK");
        }
        foreach (var f in AsteroidFields)
        {
            Debug.DrawLine(transform.position,f.transform.position,Color.white,5f, false);
        }
        //drawFriends();

	}

    void FixedUpdate()
    {

    }

    void MakeFriends()
    {
        
    }

    public void AddAsteroidField(GameObject rootGameObject)
    {
        AsteroidFields.Add(rootGameObject);
    }
}
