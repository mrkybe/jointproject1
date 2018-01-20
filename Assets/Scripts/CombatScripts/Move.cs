using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public float speed = 10f;
    public float rotateSpeed = 1f;
   
	private Rigidbody rb;
	private Vector3 lookPos;
    // Use this for initialization
    void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

	void Update()
	{
		timeMove ();
		//mouseLook ();
	}
	// Update is called once per frame
	void FixedUpdate ()
    {
		//forceMove ();
    }

	//movement based on time
	void timeMove()
	{
		float x = Input.GetAxis ("Horziontal");
		float y = Input.GetAxis("Vertical");

		transform.Translate (x * Time.deltaTime * speed, 0f, y * Time.deltaTime * speed, Space.World);

		float rx = Input.GetAxis("HorizontalR");
		float ry = Input.GetAxis("VerticalR");

		float angle = Mathf.Atan2 (rx, ry);

		transform.rotation = Quaternion.EulerAngles (0, angle, 0);
	}

	//movement based on forces, requires and FixedUpdate
	void forceMove()
	{
		float x = Input.GetAxis ("Horziontal");
		float y = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(x,0,y);
		rb.AddForce (movement * speed / Time.deltaTime);
	}

	// rays to use mouse for rotation, requires Update
	void mouseLook()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100)) 
		{
			lookPos = hit.point;
		}

		Vector3 lookDir = lookPos - transform.position;
		lookDir.y = 0;
		transform.LookAt (transform.position + lookDir, Vector3.up);
	}
		
}
