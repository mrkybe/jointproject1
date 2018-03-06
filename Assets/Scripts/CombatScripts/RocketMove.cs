using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMove : MonoBehaviour {
	public float speed = 1f;
	private ParticleSystem ps; 
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 10f);
		GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
		ps = GetComponent<ParticleSystem> ();
		ps.Stop ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag ("Enemy")) {
			ps.Play ();
		} else if (col.gameObject.CompareTag ("Asteroid")) {

		}
	}

}
