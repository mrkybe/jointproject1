using UnityEngine;
using System.Collections;

public class Enemy_Done_Mover : MonoBehaviour
{
	public float speed;
	private float startTime;

	void Start ()
	{
		startTime = Time.time;
		Destroy (gameObject, 3f);
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}

	void Update()
	{
		Waver ();
	}

	void Waver()
	{
		transform.Rotate(Mathf.Sin((Time.time-startTime+1.3f)*6)*1,0,0);
	}
}
