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
        get { return Mathf.Clamp(control_stickDirection.y, -1f, 1f); }
    }

    public float Turning
    {
        get { return Mathf.Clamp(control_stickDirection.x, -1f, 1f); }
    }
}
