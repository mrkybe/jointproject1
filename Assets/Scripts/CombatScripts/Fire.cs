using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	public float speed = 10;
	public GameObject ammo;
	private GameObject bulletClone;
	private int bulletCount = 0;
	// Use this for initialization
	void Start ()
	{
		bulletClone = new GameObject ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (bulletCount == 1) {
				Destroy (bulletClone);
				bulletCount--;
			}
			bulletClone = Shoot ();
		}
		bulletClone.transform.Translate (transform.forward * Time.deltaTime * speed * 1);

	}

	void FindProjectile() {



	}

	GameObject Shoot()
	{
		float dist  = transform.position.z + 6f;
		GameObject clone = Instantiate(ammo, new Vector3(transform.position.x, transform.position.y, dist), transform.rotation);
		bulletCount++;
		return clone;
	}
}
