using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public float speed = 10f;
	public float rotateSpeed = .05f;
   
	private Rigidbody rb;
	private Vector3 lookPos;
	private bool mouse;
    private bool controller;
    private Vector3 velocity;
    // Use this for initialization
    void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        mouse = true;
    }

	void Update()
	{
		timeMove ();
		mouseLook ();
        stickLook();
        checkCont();
    }
	// Update is called once per frame
	void FixedUpdate ()
    {
        //forceMove ();
        rb.velocity = velocity;
    }

	//movement based on time
	void timeMove()
	{
		float x = Input.GetAxisRaw("Horizontal");
		float z = Input.GetAxisRaw("Vertical");

		Vector3 move = new Vector3 (x, 0, z);
        velocity = move * speed;
	}

	//movement based on forces, requires and FixedUpdate
	void forceMove()
	{
		float x = Input.GetAxis ("Horizontal");
		float y = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(x,0,y);
		rb.AddForce (movement * speed / Time.deltaTime);
	}

	// rays to use mouse for rotation, requires Update
	void mouseLook()
	{
		if (mouse) 
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			Plane ground = new Plane (Vector3.up, Vector3.zero);
			float raylength;

			if (ground.Raycast (ray, out raylength)) 
			{
				Vector3 pointToLook = ray.GetPoint (raylength);
				Debug.DrawLine (ray.origin, pointToLook, Color.magenta);
				transform.LookAt (new Vector3 (pointToLook.x, transform.position.y, pointToLook.z));
			}
		}
	}

    void stickLook()
    {
        float rx = Input.GetAxis("HorizontalR");
        float rz = Input.GetAxis("VerticalR");
        Vector3 direction = Vector3.right * rx + Vector3.forward * rz;
        if(direction.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        //float angle = Mathf.Atan2 (rx, rz);
        //transform.rotation = Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0);
    }

	void checkCont()
	{
		Debug.Log (Input.GetJoystickNames());
	}
		
}
