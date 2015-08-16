using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShipInternals;

public class Planet : Static
{
    int Population;
    int RawMaterial;
    [SerializeField]
    private List<Object> WorkerShips;

    [SerializeField]
    private Timer TimeToSpawn;

    [SerializeField]
    int MaxFriends;
    string Faction;
    private CargoHold myStorage;

    [SerializeField]
    GameObject workership1;

    private static List<GameObject> Planets;
	// Use this for initialization
	void Start ()
    {
        //Object ship = Instantiate(workership1, transform.position + new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10)), Quaternion.identity);
        //WorkerShips.Add(ship);
        Planets = new List<GameObject>();

        MaxFriends = 3;

        TimeToSpawn = gameObject.AddComponent<Timer>();
        TimeToSpawn.SetTimer(1);
        TimeToSpawn.Loop(true);

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
        
        //drawFriends();

	}

    void FixedUpdate()
    {

    }

    void MakeFriends()
    {
        
    }
}
