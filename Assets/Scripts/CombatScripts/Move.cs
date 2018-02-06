using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public float speed = 10f;
	public float rotateSpeed = .05f;
   
	private Rigidbody rb;
	private Vector3 lookPos;
	private bool mouse;
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
		//checkCont ();
	}
	// Update is called once per frame
	void FixedUpdate ()
    {
		//forceMove ();

    }

	//movement based on time
	void timeMove()
	{
		float x = Input.GetAxis ("Horizontal");
		float z = Input.GetAxis("Vertical");

		Vector3 move = new Vector3 (x, 0, z);

		transform.position += move * speed / Time.deltaTime;

		float rx = Input.GetAxis("HorizontalR");
		float rz = Input.GetAxis("VerticalR");

		float angle = Mathf.Atan2 (rx, rz) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.EulerAngles (0,angle * rotateSpeed,0);
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

	void checkCont()
	{
		Debug.Log (Input.GetJoystickNames());
	}
		
}
