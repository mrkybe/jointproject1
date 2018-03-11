using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : MonoBehaviour {

	public GameObject Player;
	public GameObject DetectionRange;
	public float radius = 10;


	public float speed;
	public Transform shotSpawn;
	public float fireRate;
	public GameObject shot;
	private float nextFire = 2;



	private bool InRange = false;

	void Start ()
	{
		SphereCollider sphereCollider = DetectionRange.GetComponent<SphereCollider> ();
		sphereCollider.radius = radius; 
		Player = GameObject.Find ("Combat_ship_player");
	}





	void Update ()
	{
		if (InRange) {

//
//
//	



		}
		else {
			//gameObject.GetComponent<AI_Enemy> ().enabled = true;
		}
	}
//	public void OnTriggerEnter (Collider col)
//	{
//		//Debug.Log (col.name);
//		if (col.name == "Combat_ship_player")
//		{   
//			Debug.Log("Player in range");
//			InRange = true; 
//			gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
//
//			gameObject.transform.LookAt (Player.transform);
//
//
//		}
//	}
//
//	public void OnTriggerStay (Collider col)
//	{
//		if (col.name == "Combat_ship_player") {
//			
//			Debug.Log("11");
//		}
//	}
//
//	public void OnTriggerExit (Collider col)
//	{
//
//		InRange = false;
//		//Debug.Log("Player is out of range");
//	}
		
}
