using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour {

	public GameObject Player;
	public GameObject DetectionRange;
	public float radius = 10;


	private bool InRange = false;

	void Start ()
	{
		 
		SphereCollider sphereCollider = DetectionRange.GetComponent<SphereCollider> ();
		sphereCollider.radius = radius; 
	}





	void Update ()
	{
		if (InRange) {
			gameObject.GetComponent<AI_Enemy> ().enabled = false;
			gameObject.transform.LookAt (Player.transform);
		}
		else {
			gameObject.GetComponent<AI_Enemy> ().enabled = true;
		}
	}


	public void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.CompareTag ("Player"))
		{
			InRange = true; 

	
			Debug.Log("Player in range");
		}
	}

	public void OnTriggerExit (Collider col)
	{
		
		Debug.Log("Player is out of range");
	}
		
}
