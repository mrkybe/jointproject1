using UnityEngine;
using System.Collections;

public class Spaceship : Mobile
{

    private float engineRunSpeed;
    [SerializeField]
    private float engineAcceleration;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float turningSpeed;
    [SerializeField]
    private float manuverability;

	// Use this for initialization
	new void Start ()
    {
        base.Start();
        engineRunSpeed = 0;
	}

    void Update()
    {

    }
	
	// Update is called once per frame
	new void FixedUpdate ()
    {
        if (inTime && pilot)
        {
            base.FixedUpdate();
            direction = transform.forward;
            velocity = engineRunSpeed;

            if (pilot.Throttle > 0)
            {
                engineRunSpeed += engineAcceleration * pilot.Throttle;
            }
            else if (pilot.Throttle < 0)
            {
                engineRunSpeed += engineAcceleration * pilot.Throttle;
            }

            engineRunSpeed = Mathf.Clamp(maxSpeed, -(maxSpeed / 15), engineRunSpeed);
            transform.Rotate(Vector3.up * getVelocityPercentage() * (pilot.Turning * turningSpeed));
            Debug.Log(engineRunSpeed);
        }
	}

    private float getVelocityPercentage()
    {
        return (manuverability + Mathf.Abs(engineRunSpeed - maxSpeed) * (1-manuverability));
    }
}
