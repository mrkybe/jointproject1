using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	public float speed = 10;
	public GameObject ammo;
	private GameObject bulletClone;
	// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (bulletClone == true) {
				Destroy (bulletClone);
			}
			Shoot ();
		}
		bulletClone.transform.Translate (transform.forward * Time.deltaTime * speed * -1);

	}

	void FindProjectile() {



	}

	void Shoot()
	{
		float dist  = transform.position.z + 6f;
		bulletClone = Instantiate(ammo, new Vector3(transform.position.x, transform.position.y, dist), transform.rotation);

	}
}
