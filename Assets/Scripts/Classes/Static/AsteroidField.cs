using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EntityParts;

public class AsteroidField : Static
{
    int rawMaterial;
    [SerializeField]
    private CargoHold myStorage;
	// Use this for initialization
	void Start ()
    {
        myStorage = new CargoHold(50);
        myStorage.addHoldType("Ice");
        myStorage.addHoldType("Rock");
        myStorage.addHoldType("Iron");
        myStorage.addHoldType("Titanium");
        myStorage.addHoldType("Gold");
        myStorage.printHold();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        //drawFriends();

	}
}
