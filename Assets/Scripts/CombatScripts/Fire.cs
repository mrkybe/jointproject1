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
		bulletClone = new GameObject ();
		rb = new Rigidbody ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButtonDown("Fire1") && bulletCount == 0) 
		{
			Shoot ();
			if (bulletCount == 1) 
			{
				Destroy (bulletClone);
				bulletCount--;
			}
		}
		rb.AddForce(new Vector3(transform.position.x, transform.position.y * speed, transform.position.z), ForceMode.Acceleration);	

	}
		

	void Shoot()
	{
		float dist  = transform.position.z + 6f;
		bulletCount++;
		bulletClone = Instantiate(ammo, new Vector3(transform.position.x, transform.position.y, dist), transform.rotation);
        rb = bulletClone.GetComponent<Rigidbody>();

	}
}
