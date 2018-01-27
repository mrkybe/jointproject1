using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	public float speed = 2;
	public GameObject ammo;
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
		}
	}
		
	void Shoot()
	{
		Debug.Log ("shoot");
        //float dist  = transform.position.z + 3f;
        GameObject bulletClone = Instantiate(ammo, transform.position, transform.rotation);
        rb = bulletClone.GetComponent<Rigidbody>();
        // rb.AddForce(new Vector3(transform.position.x, transform.position.y, transform.position.z * speed), ForceMode.Force);
        //rb.AddForce(rb.transform.forward * speed);
        rb.velocity = bulletClone.transform.forward * speed;
        Destroy(bulletClone, 1.5f);
    }
}
