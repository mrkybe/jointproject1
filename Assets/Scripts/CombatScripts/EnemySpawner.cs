using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Debugger Controls
	You can expand the spawnPoints parameter, and change the Size to the number
	  of desired spawn points.  For each point, enter the coordinates for spawn positions
	
	Change the spawnCooldown to the desired time between enemy spawns
*/
public class EnemySpawner : MonoBehaviour {

	public GameObject[] hazards;
	public Vector3 spawnValues;
	public int hazardCount; 
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public int totalAmount = 100;
	public int count = 0;


	void Start () {
		count = 0;
		//StartSpawn ();
	}


	///<summary>
	/// Spawning enemies waves at random locations around the player. Different enemies are defined in the hazards.
	///</summary>
	IEnumerator SpawnWaves ()
	{

		yield return new WaitForSeconds (startWait);
		while (true)
		{
			for (int i = 0; i < hazardCount; i++) 
			{
				GameObject hazard = hazards[Random.Range(0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x+transform.position.x, spawnValues.x+transform.position.x), transform.position.y, Random.Range (-spawnValues.z+transform.position.z, spawnValues.z+transform.position.z));
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				count = count+3;

				if (totalAmount < count) {
				
					yield break;
				}
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);

		}
	}

	public void Enable() {
		enabled = true;
	}

	public void Disable() {
		enabled = false; 
	}

	public void Stop()
	{
		StopAllCoroutines ();
	}

	public void StartSpawn()
	{
		StartCoroutine (SpawnWaves ());
	}
}
