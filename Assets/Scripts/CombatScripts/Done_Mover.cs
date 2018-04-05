using UnityEngine;
using System.Collections;

public class Done_Mover : MonoBehaviour
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

	//wavers bullet
	void Waver()
	{
		float x = transform.position.x;
		float z = transform.position.z;
		/*
		int rand = Random.Range (0, 99);
		float randDecimal = rand / 100;
		x += rand;
		z += rand;

		rand = Random.Range (0, 99);
		randDecimal = rand / 100;
		x -= rand;
		z -= rand;
		*/
		transform.Rotate(Mathf.Sin((Time.time-startTime+1.3f)*6)*1,0,0);
	}

}
