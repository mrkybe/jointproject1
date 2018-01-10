using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFire : MonoBehaviour {
    private LineRenderer laser;
    // Use this for initialization
	void Start ()
    {
        laser = this.gameObject.GetComponent<LineRenderer>();
        laser.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetButtonDown("Fire1"))
        {
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
        }	
	}

    IEnumerator FireLaser()
    {
        laser.enabled = true;
        while (Input.GetButton("Fire1"))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            laser.SetPosition(0, ray.origin);
            laser.SetPosition(1, ray.GetPoint(100));

            if (Physics.Raycast(ray,out hit))
            {
                Debug.Log("hit:");
                if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    Destroy(hit.transform.gameObject);
                }
            }   

            yield return null;
        }
        laser.enabled = false;
    }
}
