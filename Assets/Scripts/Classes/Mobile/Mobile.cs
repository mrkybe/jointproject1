using UnityEngine;
using System.Collections;

public class Mobile : MonoBehaviour
{
    /* Written by Ruslan Kaybyshev, Late 2013
     * Purpose: Define the base class for all entities that will be moving
     *          every fixed update.  This class will be derived to create
     *          ships, asteroids, bullets, missles.  Lasers are instant
     *          (probably) and therefore are not derived from moveable. (probably)
     */
    protected Vector3 direction;
    protected float velocity;
    static public bool inTime;
    // Use this for initialization
    protected void Start()
    {
        inTime = true;
	}
	
	// Update is called once per frame
    protected void FixedUpdate()
    {
        if (inTime)
        {
            Move();
        }
	}

    private void Move()
    {
        transform.position = transform.position + ((velocity * direction.normalized) * Time.deltaTime * 5);
    }
}
