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

    private List<GameObject> AsteroidList;
	// Use this for initialization
    void Awake()
    {
        AsteroidList = new List<GameObject>();
        origin = transform.position;
    }


	void Start () {
	}

    public void SetupCombatAsteroids()
    {
        GenerateAsteroids(count);
    }

    public void HideAsteroids()
    {
        foreach (var a in AsteroidList)
        {
            a.transform.position = new Vector3(0,10000,0);

            a.SetActive(false);
        }
    }

	private void GenerateAsteroids (int count)
	{
		GameObject prefab = asteroidPrefab;
		Vector3 pos = Vector3.zero;
		for (int i = 0; i < count; i++)
		{
		    GameObject createdAsteroid;
            if (AsteroidList.Count > i && AsteroidList.Count < count)
            {
                createdAsteroid = AsteroidList[i];
                createdAsteroid.SetActive(true);
            }
            else
            {
                createdAsteroid = Instantiate(prefab, Vector3.zero, Random.rotation);
                AsteroidList.Add(createdAsteroid);
            }

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
