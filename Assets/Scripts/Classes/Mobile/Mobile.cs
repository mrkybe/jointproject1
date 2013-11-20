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
    [SerializeField]
    protected bool isPlayer;
    [SerializeField]
    protected bool isAI;
    protected PilotInterface pilot;
    static public bool inTime;
    // Use this for initialization
    protected void Start()
    {
        inTime = true;
        if (pilot == null && isPlayer)
        {
            pilot = gameObject.AddComponent<PlayerPilot>();
        }
        else if(pilot == null && isAI)
        {
            pilot = gameObject.AddComponent<AI_Gather>();
        }
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
        transform.position = transform.position + ((velocity * direction.normalized) * Time.deltaTime);
    }
}
