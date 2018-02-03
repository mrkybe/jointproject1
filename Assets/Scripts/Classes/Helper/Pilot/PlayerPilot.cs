using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Classes.WorldSingleton;

public class PlayerPilot : PilotInterface
{
    private Spaceship myShip = null;
	// Use this for initialization
	new void Start ()
    {
        base.Start();
        myShip = GetComponent<Spaceship>();
        Faction myFaction = Overseer.Main.GetFaction("Player");
        myShip.Faction = myFaction;
    }
	
	// Update is called once per frame
	new void Update ()
    {
        base.Update();
        control_stickDirection.x = Input.GetAxis("Horizontal"); // turning
        control_stickDirection.y = Input.GetAxis("Vertical");   // throttle
    }

    public override void Die()
    {
        // Game Over!
        throw new NotImplementedException();
    }
}
