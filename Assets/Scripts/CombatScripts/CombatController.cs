using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour {

	public GameObject[] hazards;
	public Vector3 spawnValues;
	public GameObject player;  
	public int hazardCount; 
	public float spawnWait;
	public float startWait;
	public float waveWait;
	// Use this for initialization
	void Start () {
		StartCoroutine (SpawnWaves ());


	}
	IEnumerator SpawnWaves ()
	{

		yield return new WaitForSeconds (startWait);
		while (true)
		{
			for (int i = 0; i < hazardCount; i++) 
			{

				GameObject hazard = hazards[Random.Range(0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRptation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRptation);

				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);

		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
