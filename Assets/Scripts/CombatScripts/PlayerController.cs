using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Rigidbody rb;

	public GameObject enemySpawner;
	public GameObject cameraObject;
	public GameObject combatField;
	public float speed = 10f;
	private bool flag = false;

	public float rotateSpeed = .05f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();


	}


	void FixedUpdate()  // called each physics steps
	{
		
		if (Input.GetKey (KeyCode.C)&&flag ==false) {
			flag = true;
			transform.position = new Vector3 (combatField.transform.position.x, combatField.transform.position.y, combatField.transform.position.z);

			cameraObject.transform.position = new Vector3 (cameraObject.transform.position.x, cameraObject.transform.position.y+20, cameraObject.transform.position.z);
			combatField.SetActive (true);
			enemySpawner.GetComponent<EnemySpawner>().enabled = true;


		}

		float moveHorizontal = Input.GetAxis ("Horizontal"); // default axis : Horizontal, vertical
		float moveVertical = Input.GetAxis ("Vertical");
		float rotateHorizontal = Input.GetAxis ("HorizontalR");
		float rotateVertical = Input.GetAxis ("VerticalR");
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		transform.Rotate (0, Mathf.Atan2 (rotateHorizontal, rotateVertical) * Mathf.Rad2Deg * rotateSpeed, 0);


		rb.velocity = movement*speed;


	}
}
