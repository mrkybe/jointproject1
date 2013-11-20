using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EntityParts;

public class AsteroidField : Static
{
    public static List<AsteroidField> listOfAsteroidFields;
    int rawMaterial;
    [SerializeField]
    private CargoHold myStorage;
    [SerializeField]
    private int maxStorage = 10000000;
	// Use this for initialization
	void Start ()
    {
        base.Start();
        myStorage = new CargoHold(maxStorage);
        listOfAsteroidFields = new List<AsteroidField>();
        listOfAsteroidFields.Add(this);
	}

    new protected void DelayedLoad()
    {
        myStorage.addHoldType("Ice");
        myStorage.addHoldType("Rock");
        myStorage.addHoldType("Iron");
        myStorage.addHoldType("Titanium");
        myStorage.addHoldType("Gold");
        myStorage.addToHold("Ice", 1000000);
        myStorage.addToHold("Rock", 5000000);
        myStorage.addToHold("Iron", 500000);
        myStorage.addToHold("Titanium", 90000);
        myStorage.addToHold("Gold", 15000);
        //Debug.Log("PRINTING HOLD FOR ASTEROID FIELD");
        //myStorage.printHold();
        //Debug.Log("-NOTE: STAGE " + loadPriorityInital + " LOADING COMPLETE");
    }
	// Update is called once per frame
	new void FixedUpdate ()
    {
        if (inTime)
        {
            base.FixedUpdate();
            ///////////////////
            if (loadPriority == 0)
            {
                DelayedLoad();
                loadPriority = -1;
            }
            else if (loadPriority > 0)
            {
                loadPriority--;
            }
        }
	}
}
