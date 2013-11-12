using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EntityParts;

public class Planet : Static
{
    int population;
    int rawMaterial;
    [SerializeField]
    private GameObject workerShip;
    [SerializeField]
    private Timer timeToSpawn;
    [SerializeField]
    int maxFriends;
    string faction;
    private CargoHold myStorage;

    private List<GameObject> closeBuddies;
	// Use this for initialization
	void Start ()
    {
        CargoItemTypes.AddCargoItemType(new CargoItem("Fish"));
        CargoItemTypes.AddCargoItemType(new CargoItem("Cow"));
        CargoItemTypes.AddCargoItemType(new CargoItem("Milk"));
        CargoItemTypes.AddCargoItemType(new CargoItem("Ammo"));
        CargoItemTypes.AddCargoItemType(new CargoItem("Carrots"));

        maxFriends = 3;

        timeToSpawn = gameObject.AddComponent<Timer>();
        timeToSpawn.SetTimer(1);
        timeToSpawn.Loop(true);

        CargoItemTypes.AddCargoItemType(new CargoItem("Fish"));
        myStorage = new CargoHold(50);
        myStorage.addHoldType("Fish");
        myStorage.printHold();
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
