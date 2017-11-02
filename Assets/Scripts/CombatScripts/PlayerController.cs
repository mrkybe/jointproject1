using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Rigidbody rb;
	public float speed = 10f;
	public float rotateSpeed = .05f;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();

	}


	void FixedUpdate()  // called each physics steps
	{

		float moveHorizontal = Input.GetAxis ("Horizontal"); // default axis : Horizontal, vertical
		float moveVertical = Input.GetAxis ("Vertical");
		float rotateHorizontal = Input.GetAxis ("HorizontalR");
		float rotateVertical = Input.GetAxis ("VerticalR");
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		transform.Rotate (0, Mathf.Atan2 (rotateHorizontal, rotateVertical) * Mathf.Rad2Deg * rotateSpeed, 0);


		rb.velocity = movement*speed;


	}
}
