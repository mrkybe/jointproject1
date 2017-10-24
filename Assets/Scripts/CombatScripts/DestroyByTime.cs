using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {
	public float lifetime;
	// Use this for initialization
	void Start () {
		// Use this for initialization
		if(gameObject != null)
		{
		Destroy (gameObject, lifetime);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
