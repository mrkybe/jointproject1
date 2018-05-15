using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class As : MonoBehaviour {


	[SerializeField]
	private GameObject[] asteroids;
	[SerializeField]
	private float size = 5f; 

	[SerializeField]
	private float maxDistance = 5f;
	[SerializeField]
	private float minDistance = 0f;

	private Vector3 origin = Vector3.zero;
	// Use this for initialization
	void Start () {
		origin = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void GenerateAsteroids (int count)
	{
		GameObject prefab;
		Vector3 pos = Vector3.zero;
		for (int i = 0; i < count; i++) {
			prefab = asteroids [Random.Range (0, asteroids.Length)];

			for (int j = 0; j < 100; j++) {
				pos = Random.insideUnitSphere * (minDistance + (maxDistance - minDistance) * Random.value);

				pos += origin;
				pos.y = origin.y;

				if (!Physics.CheckSphere (pos, size / 2.0f)) {
					break;
				}
			}
			Instantiate (prefab, pos, Random.rotation);

		}



	}
}
