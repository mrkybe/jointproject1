﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMove : MonoBehaviour {
	public float speed = 1f;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 10f);
		GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
