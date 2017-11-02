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


	void Update()  // called each physics steps
	{

		float moveHorizontal = Input.GetAxis ("Horizontal"); // default axis : Horizontal, vertical
		float moveVertical = Input.GetAxis ("Vertical");
		float rotateHorizontal = Input.GetAxis ("HorizontalR");
		float rotateVertical = Input.GetAxis ("VerticalR");
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		transform.eulerAngles = new Vector3( 0, Mathf.Atan2( rotateVertical, rotateHorizontal) * Mathf.Rad2Deg, 0 );
		//new vector3(char.transform.eulerAngles.x, Mathf.atan2(x, y) * Mathf.rad2deg, char.transform.eulerAngles.z);
		rb.velocity = movement*speed;


	}
}
