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


	void OnTriggerEnter(Collider colidedObj)
	{
		Debug.Log (colidedObj.name);
		if (colidedObj.name == "Roamer(Clone)") {
			Destroy (gameObject);
			Destroy (colidedObj.gameObject);
		
		}

	
	}
}
