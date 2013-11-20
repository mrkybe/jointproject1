using UnityEngine;
using System.Collections;

public class PlayerPilot : PilotInterface
{

	// Use this for initialization
	new void Start ()
    {
        base.Start();
	}
	
	// Update is called once per frame
	new void Update ()
    {
        base.Update();
        control_stickDirection.x = Input.GetAxis("Horizontal"); // turning
        control_stickDirection.y = Input.GetAxis("Vertical");   // throttle
	}
}
