using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyByContact : MonoBehaviour {

    public GameObject bullet;
	// Use this for initialization
	void Start () {
		
	}
	//<summary>
	// When bullet hits the enemy, destory both bullet and enemy objects.
	//</summary>
	void OnTriggerEnter(Collider other) {
		//other.gameObject.GetComponent<DestroyByTime>().enabled = false;
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
		Destroy (bullet);
	}
}
