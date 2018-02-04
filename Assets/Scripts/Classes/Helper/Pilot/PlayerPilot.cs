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

        //Vector3.RotateTowards(targetFaceDirection);
        //Quaternion rot = Quaternion.Euler(Input.GetAxis("Horizontal") * Vector3.up);
        Quaternion rot = Quaternion.Euler(Input.GetAxis("Horizontal") * Vector3.up * Time.deltaTime * 120f);
        targetFaceDirection = rot * targetFaceDirection;

        float angle;
        Vector3 direction;
        Debug.DrawLine(transform.position, transform.position + targetFaceDirection * 5f);
    }

    public override void Die()
    {
        // Game Over!
        throw new NotImplementedException();
    }
}
