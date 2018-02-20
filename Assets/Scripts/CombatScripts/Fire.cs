using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Fire class is responsible for creating and firing a projectile forward when the player hits the fire button
///</summary>
public class Fire : MonoBehaviour {

	public float speed;
	public GameObject bullet;
	public GameObject laser;
	public GameObject ammo;
	private int bulletCount = 0;
	private Rigidbody rb;
	void Start()
	{
		ammo = bullet;
		speed = 3f;
	}
	///<summary>
	/// Check every frame if the fire button has been pressed, then call the Shoot function.
	///</summary>
	void Update () 
	{
		if (Input.GetButtonDown("Fire1")) 
		{
			Shoot ();
		}

	}
    ///<summary>
	/// Shoot function sends a message to console saying "shoot" and then creates a bullet using Unity's Instantiate() function.
	/// The bullet fired is assigned in the inspector view in Unity. The bullet is created at the player's location and is then 
	/// shot forward by adding to the bullet's velocity.
	///</summary
	void Shoot()
	{
		Debug.Log ("shoot");
        float dist  = transform.position.z + 3f;
		GameObject clone = Instantiate(ammo, transform.position, transform.rotation);
        rb = clone.GetComponent<Rigidbody>();
        // rb.AddForce(new Vector3(transform.position.x, transform.position.y, transform.position.z * speed), ForceMode.Force);
        //rb.AddForce(rb.transform.forward * speed);
        //rb.velocity = clone.transform.forward * speed;
		rb.AddForce (clone.transform.forward * speed, ForceMode.Impulse);
        Destroy(clone, 1.5f);
    }
}
