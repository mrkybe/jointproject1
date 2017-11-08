using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	public float speed = 1;
	public GameObject ammo;
	private GameObject bulletClone;
	private int bulletCount = 0;
	private Rigidbody rb;
	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButtonDown("Fire1")) 
		{
			Shoot ();
            /*
			if (bulletCount == 1) 
			{
				Destroy (bulletClone);
				bulletCount--;
			}
            */
		}
		

	}
		

	void Shoot()
	{
		float dist  = transform.position.z + 3f;

        bulletClone = Instantiate(ammo, new Vector3(transform.position.x, transform.position.y, dist), transform.rotation);
        rb = bulletClone.GetComponent<Rigidbody>();
       // rb.AddForce(new Vector3(transform.position.x, transform.position.y, transform.position.z * speed), ForceMode.Force);
        rb.AddForce(rb.transform.forward * speed);
        //rb.velocity = bulletClone.transform.forward * speed;

        Destroy(bulletClone, 2.0f);
    }
}
