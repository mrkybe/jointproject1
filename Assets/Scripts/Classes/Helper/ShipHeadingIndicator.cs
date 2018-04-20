using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes.Helper.Pilot;
using UnityEngine;

public class ShipHeadingIndicator : MonoBehaviour
{
    private PilotInterface myPilot = null;
	// Use this for initialization
	void Start ()
	{
	    myPilot = GetComponentInParent<PilotInterface>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (myPilot != null)
        {
            this.transform.LookAt(myPilot.TargetFaceDirection + transform.position, Vector3.up);
        }
	}
}
