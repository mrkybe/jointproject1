using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	public float speed = 1;
	public GameObject ammo;
	private GameObject bulletClone;
<<<<<<< HEAD
	private int bulletCount = 0;
=======
    private Rigidbody rb;
>>>>>>> origin/combat
	// Use this for initialization
	void Start ()
	{
		bulletClone = new GameObject ();
        rb = new Rigidbody();
	}

	// Update is called once per frame
	void Update () {
<<<<<<< HEAD
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (bulletCount == 1) {
=======
		if (Input.GetButtonDown("Fire1")) {
			if (bulletClone == true) {
>>>>>>> origin/combat
				Destroy (bulletClone);
				bulletCount--;
			}
			Shoot ();
		}
<<<<<<< HEAD
		bulletClone.transform.Translate (transform.forward * Time.deltaTime * speed * 1);
=======
        rb.AddForce(new Vector3(transform.position.x, transform.position.y * speed, transform.position.z), ForceMode.Force);
>>>>>>> origin/combat

	}

	void FindProjectile() {



	}

	void Shoot()
	{
		float dist  = transform.position.z + 6f;
<<<<<<< HEAD
		GameObject clone = Instantiate(ammo, new Vector3(transform.position.x, transform.position.y, dist), transform.rotation);
		bulletCount++;
		return clone;
=======
		bulletClone = Instantiate(ammo, new Vector3(transform.position.x, transform.position.y, dist), transform.rotation);
        rb = bulletClone.GetComponent<Rigidbody>();
>>>>>>> origin/combat
	}
}
