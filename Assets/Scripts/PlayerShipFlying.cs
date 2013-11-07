using UnityEngine;
using System.Collections;

public class PlayerShipFlying : MonoBehaviour
{
    private float throttle;
    [SerializeField]
    private float maxThrottle;

	// Use this for initialization
	void Start ()
    {
        throttle = 0;
        maxThrottle = 1;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if( throttle > 0 )
        transform.position = transform.position + ((throttle * transform.forward) / 10);
        if (Input.GetKey(KeyCode.W))
        {
            throttle += 0.05f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            throttle -= 0.05f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up * -1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up);
        }
        throttle = Mathf.Clamp(throttle, 0, maxThrottle);
	}
}
