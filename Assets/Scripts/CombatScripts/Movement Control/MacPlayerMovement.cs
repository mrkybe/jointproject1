using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacPlayerMovement : MonoBehaviour {
	[SerializeField]
	private float speed = 5f;

	private Rigidbody myRigidbody;



	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody> ();
			
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			myRigidbody.velocity = new Vector3 (-speed, myRigidbody.velocity.y, myRigidbody.velocity.z);
		}
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			myRigidbody.velocity = new Vector3 (speed, myRigidbody.velocity.y, myRigidbody.velocity.z);
		}
		if(Input.GetKeyDown(KeyCode.UpArrow)){
			myRigidbody.velocity = new Vector3 (myRigidbody.velocity.x, myRigidbody.velocity.y, speed);
		}
		if(Input.GetKeyDown(KeyCode.DownArrow)){
			myRigidbody.velocity = new Vector3 (myRigidbody.velocity.x, myRigidbody.velocity.y, -speed);
		}
	}
}
