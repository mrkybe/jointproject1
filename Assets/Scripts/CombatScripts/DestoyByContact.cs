using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyByContact : MonoBehaviour {
 

	// Use this for initialization
	void Start () {
		
	}
	void OnTriggerEnter(Collider other) {
		gameObject.GetComponent<DestroyByTime>().enabled = false;

		Destroy(other.gameObject);
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
