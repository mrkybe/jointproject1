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
    // Use this for initialization
    protected void Start()
    {
	
	}
	
	// Update is called once per frame
    protected void Update()
    {
        Move();
	}

    private void Move()
    {
        transform.position = transform.position + ((velocity * direction.normalized) * Time.deltaTime * 5);
    }
}
