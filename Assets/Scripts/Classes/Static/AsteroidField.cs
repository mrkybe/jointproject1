using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EntityParts;

public class AsteroidField : Static
{
    int rawMaterial;
    private CargoHold myStorage;
    private int maxStorage = 200;
	// Use this for initialization
	void Start ()
    {
        base.Start();
        myStorage = new CargoHold(maxStorage);
	}

    new protected void DelayedLoad()
    {
        myStorage.addHoldType("Gold");
        myStorage.addToHold("Gold", 200);
        //Debug.Log("PRINTING HOLD FOR ASTEROID FIELD");
        
        //Debug.Log("-NOTE: STAGE " + loadPriorityInital + " LOADING COMPLETE");
    }

    public CargoHold GetCargoHold
    { 
        get { return myStorage; } 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            myStorage.printHold();
        }
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
