using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;       //Public variable to store a reference to the player game object
	public float y_value = 10f;//Private variable to store the offset distance between the player and camera

	private float y_distance;
	// Use this for initialization
	void Start () 
	{
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		y_distance = player.transform.position.y + y_value;
		transform.rotation = Quaternion.Euler (90, 0, 0);
	}

	// LateUpdate is called after Update each frame
	void FixedUpdate () 
	{
		y_distance = player.transform.position.y + y_value;
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		transform.position = new Vector3(player.transform.position.x, y_distance, player.transform.position.z);
	}
}

