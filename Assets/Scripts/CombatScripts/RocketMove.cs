using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMove : MonoBehaviour {
	public float speed = 1f;
	private ParticleSystem ps; 
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 10f);
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}
}
