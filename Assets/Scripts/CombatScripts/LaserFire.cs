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

            if (Physics.Raycast(ray, out hit, 100))
            {
                laser.SetPosition(1, hit.point);

                if (hit.rigidbody.gameObject.CompareTag("Enemy"))
                {
                    hit.rigidbody.AddForceAtPosition(transform.forward* 10, hit.point);
                    Destroy(hit.rigidbody.gameObject);
                }
            }
            else
                laser.SetPosition(1, ray.GetPoint(100));

            yield return null;
        }

        laser.enabled = false;
    }
}
