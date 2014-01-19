using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EntityParts;

public class Planet : Static
{
    int population;
    int rawMaterial;
    [SerializeField]
    private List<Object> workerShips;
    [SerializeField]
    private Timer timeToSpawn;
    [SerializeField]
    int maxFriends;
    string faction;
    private CargoHold myStorage;
    [SerializeField]
    GameObject workership1;

    private List<GameObject> closeBuddies;
	// Use this for initialization
	void Start ()
    {
        Object ship = Instantiate(workership1, transform.position + new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10)), Quaternion.identity);
        workerShips.Add(ship);


        maxFriends = 3;

        timeToSpawn = gameObject.AddComponent<Timer>();
        timeToSpawn.SetTimer(1);
        timeToSpawn.Loop(true);

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
        if (timeToSpawn.Done)
        {
            //Debug.Log("TICK TOCK");
        }
        
        //drawFriends();

	}

    void FixedUpdate()
    {

    }

    void drawFriends()
    {
        for (int j = 0; j < closeBuddies.Count; j++)
        {
            Debug.DrawLine(closeBuddies[j].transform.position+ new Vector3(0,0.6f,0), transform.position, Color.blue, 5f);
            //Debug.Log("Drawing Friends" + j);
        }
    }

    void makeFriends()
    {
        Debug.Log("makin amiagos");
        GameObject[] canidates = GameObject.FindGameObjectsWithTag("StaticInteractive");
        Debug.Log("canidates: " + canidates.Length);
        closeBuddies = new List<GameObject>();
        for (int i = 0; i < canidates.Length; i++)
        {
            if (closeBuddies.Count < maxFriends)
            {
                closeBuddies.Add(canidates[i]);
                Debug.Log("+friend");
            }
            else
            {
                for (int j = 0; j < closeBuddies.Count; j++)
                {
                    if ((Vector3.Distance(canidates[i].transform.position, transform.position) <
                        Vector3.Distance(closeBuddies[j].transform.position, transform.position)) &&
                        !closeBuddies.Contains(canidates[i])&&
                        closeBuddies.Contains(canidates[i])!=gameObject)
                    {
                        closeBuddies.RemoveAt(j);
                        closeBuddies.Add(canidates[i]);
                        Debug.Log("+closer friend");
                    }
                }
            }
        }
        Debug.Log("friends: " + closeBuddies.Count);
    }
}
