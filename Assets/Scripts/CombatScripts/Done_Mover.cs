using UnityEngine;
using System.Collections;

public class Done_Mover : MonoBehaviour
{
	public float speed;

	void Start ()
	{
		Destroy (gameObject, 3f);
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}
//	void OnCollisionEnter(Collision col)
//	{
//		if (col.gameObject.CompareTag ("Enemy")) {
//			Destroy (gameObject);
//			Destroy (col.gameObject);
//		}
//	}


	void OnCollisionEnter (Collision col)
	{
		//Debug.Log (colidedObj.name);

			
			//Destroy (colidedObj.gameObject);

	
	}
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag ("Player")) {
			col.gameObject.GetComponent<PlayerController> ().Depletion(10);
		}
	}
}
