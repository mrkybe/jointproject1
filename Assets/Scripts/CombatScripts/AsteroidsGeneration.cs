using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsGeneration : MonoBehaviour {


	[SerializeField]
	private GameObject asteroidPrefab;
	[SerializeField]
	private float size = 5f;
    [SerializeField]
    private int count = 100;

    [SerializeField]
	private float maxDistance = 5f;
	[SerializeField]
	private float minDistance = 0f;

	private Vector3 origin = Vector3.zero;
	// Use this for initialization
	void Start () {
		origin = transform.position;
        GenerateAsteroids(count);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void GenerateAsteroids (int count)
	{
		GameObject prefab = asteroidPrefab;
		Vector3 pos = Vector3.zero;
		for (int i = 0; i < count; i++) {
		    GameObject createdAsteroid = Instantiate(prefab, Vector3.zero, Random.rotation);
		    createdAsteroid.GetComponent<CombatAsteroid>().Initialize();
		    var bounds = createdAsteroid.GetComponent<MeshRenderer>().bounds;

            for (int j = 0; j < 100; j++) {
				pos = Random.insideUnitSphere * (minDistance + (maxDistance - minDistance) * Random.value);

				pos += origin;
				pos.y = origin.y;

				if (!Physics.CheckSphere (pos, bounds.extents.magnitude))
				{
				    createdAsteroid.transform.position = pos;
					break;
				}
			}

		}



	}
}
