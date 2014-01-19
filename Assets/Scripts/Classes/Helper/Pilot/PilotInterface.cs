using UnityEngine;
using System.Collections;

public class PilotInterface : MonoBehaviour
{
    protected Planet homePlanet;
    protected Vector2 control_stickDirection;
    protected float targetSpeed;

	// Use this for initialization
    protected void Start()
    {
        control_stickDirection = new Vector2();
        targetSpeed = 0;
	}

    protected void Update()
    {

	}

    public Vector3 Direction
    {
        get { return control_stickDirection.normalized; }
    }

    public float Throttle
    {
        get { return Mathf.Clamp(control_stickDirection.y, -1f, 1f); }
    }

    public float Turning
    {
        get { return Mathf.Clamp(control_stickDirection.x, -1f, 1f); }
    }

    public float TargetSpeed
    {
        get
        {
            //Debug.Log("Tried to get targetSpeed, gonna tell him " + targetSpeed);
            return targetSpeed;
        }
    }
}
