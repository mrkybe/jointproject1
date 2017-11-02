using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Rigidbody rb;
	public float speed = 10f;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()  // called each physics steps
	{

		float moveHorizontal = Input.GetAxis ("Horizontal"); // default axis : Horizontal, vertical
		float moveVertical = Input.GetAxis ("Vertical");
		float rotateHorizontal = Input.GetAxis ("HorizontalR");
		float rotateVertical = Input.GetAxis ("VerticalR");
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		transform.Rotate (rotateHorizontal * speed, rotateVertical * speed, 0);
		rb.velocity = movement*speed;


	}
}
