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
            throttle += 0.01f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            throttle -= 0.01f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up * -2 * getVelocityPercentage());
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * 2 * getVelocityPercentage());
        }
        throttle = Mathf.Clamp(throttle, 0, maxThrottle);
	}

    private float getVelocityPercentage()
    {
        return (0.10f + Mathf.Abs(throttle - 1)*0.90f);
    }
}
