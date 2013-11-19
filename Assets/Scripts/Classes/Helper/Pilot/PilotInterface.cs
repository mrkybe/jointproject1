using UnityEngine;
using System.Collections;

public class PilotInterface : MonoBehaviour
{
    protected Vector2 control_stickDirection;

	// Use this for initialization
    protected void Start()
    {
        control_stickDirection = new Vector2();
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
        get { return control_stickDirection.y; }
    }

    public float Turning
    {
        get { return control_stickDirection.x; }
    }
}
