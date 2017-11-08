using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyByContact : MonoBehaviour {

    public GameObject bullet;
	// Use this for initialization
	void Start () {
		
	}
	/*void OnTriggerEnter(Collider other) {
		other.gameObject.GetComponent<DestroyByTime>().enabled = false;
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
		Destroy (bullet);
	}*/

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<DestroyByTime>().enabled = false;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(bullet);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
