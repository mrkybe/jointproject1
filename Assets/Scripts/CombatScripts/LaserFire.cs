using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// LaserFire class is used for firing a laser by utilizing Unity's LineRenderer component.
///</summary>
public class LaserFire : MonoBehaviour {
    private LineRenderer laser;
	public bool rayhit;

	public AudioClip shootSound;

	private AudioSource source;


	void Awake(){

		source = GetComponent <AudioSource> ();
	}
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
            //StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
			source.PlayOneShot (shootSound);
        }

		if (!rayhit)
			StopCoroutine("FireLaser");

	}

	///<summary>
	/// IEnumerator function FireLaser is used to enable the LineRenderer and use a ray forward to check for a hit.
	/// If the ray hits an object, and if the object is an enemy, then destroy it. IEnumerator is used to create a sustained laser using coroutines. 
	///</summary>
    IEnumerator FireLaser()
    {
        
        while (Input.GetButtonDown("Fire1"))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit))
            {
				rayhit = true;
				laser.enabled = true;
				//float dist = transform.InverseTransformVector(transform.position - hit.point).magnitude;
				laser.SetPosition(0, ray.origin);
				//laser.SetPosition(1, new Vector3(0, 0, dist + hit.distance));
				laser.SetPosition(1, ray.GetPoint(hit.distance));
                //Debug.Log("hit:");
                if (hit.transform.gameObject.CompareTag("Enemy"))
                {
					int health = hit.transform.gameObject.GetComponent<AI_Enemy> ().health;
					int dmg = hit.transform.gameObject.GetComponent<AI_Enemy> ().laserDMG;
					health -= dmg;
                    //Destroy(hit.transform.gameObject);
					rayhit = false;
					//laser.enabled = false;
                }
            }  
            yield return null;
        }
        laser.enabled = false;
    }
}
