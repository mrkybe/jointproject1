namespace ShipInternals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

	public class SensorArray
	{
	    GameObject _owner;
	    Spaceship myShipScript;
	
	    public SensorArray(GameObject owner_in)
	    {
	        _owner = owner_in;
	        myShipScript = _owner.transform.GetComponent<Spaceship>();
	    }
	
	    public float StoppingDistance
	    {
	        //d = (vf^2 +  vi^2) / 2a
	        get
	        {
	            //Debug.Log("GETTING STOPPING DISTANCE~");
	            float distance = ((myShipScript.EngineRunSpeed * myShipScript.EngineRunSpeed) / (2 * myShipScript.EngineAcceleration)) * Time.fixedDeltaTime;
	            //Debug.Log("STOPPING DISTANCE IS: " + distance);
	            //Debug.Log("STOPPING TIME IS: " + stoppingTime);
	            return distance;
	        }
	    }
	
	    public float EngineRunSpeed
	    {
	        get { return myShipScript.EngineRunSpeed; }
	    }
	    public float EngineAcceleration
	    {
	        get { return myShipScript.EngineAcceleration; }
	    }
	    public float MaxSpeed
	    {
	        get { return myShipScript.MaxSpeed; }
	    }
	    public float TurningSpeed
	    {
	        get { return myShipScript.TurningSpeed; }
	    }
	
	    public float Manuverability
	    {
	        get { return myShipScript.Manuverability; }
	    }
	
	    public float TargetSpeed
	    {
	        get { return myShipScript.TargetSpeed; }
	    }
	}
}