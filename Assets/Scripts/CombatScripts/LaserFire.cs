using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// LaserFire class is used for firing a laser by utilizing Unity's LineRenderer component.
///</summary>
public class LaserFire : MonoBehaviour {
    private LineRenderer laser;
    // Use this for initialization
	void Start ()
    {
        laser = gameObject.GetComponent<LineRenderer>();
        laser.enabled = false;
	}
	
	///<summary>
	/// Check every frame if the fire button has been pressed, and starts coroutine FireLaser until the player lets go of the fir button.
	///</summary>
	void Update ()
    {
	    if (Input.GetButtonDown("Fire1"))
        {
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
        }	
	}

	///<summary>
	/// IEnumerator function FireLaser is used to enable the LineRenderer and use a ray forward to check for a hit.
	/// If the ray hits an object, and if the object is an enemy, then destroy it. IEnumerator is used to create a sustained laser using coroutines. 
	///</summary>
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
