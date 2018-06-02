using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class EnemyFire : Action
{
	
	public SharedFloat speed = 20; 

	private SharedTransform shotSpawn;
	public SharedFloat fireRate = 4;
	public SharedGameObject shot;
	private SharedFloat nextFire = 2;
	public SharedAudioSource source;


	// The transform that the object is moving towards
	public SharedTransform target;

	public override void OnAwake ()
	{
		base.OnAwake ();
		shotSpawn = this.transform.GetChild (1).transform;
		source = gameObject.GetComponent<AudioSource> ();

	}

	public override TaskStatus OnUpdate()
	{
		enemyFire (shotSpawn.Value);
		return TaskStatus.Success;
		
	}
	public void enemyFire(Transform shotSpawn)
	{
		if (Time.time > nextFire.Value) 
		{
			
			source.Value.PlayOneShot (source.Value.clip);
			nextFire.Value = Time.time + fireRate.Value;
			UnityEngine.GameObject.Instantiate(shot.Value, shotSpawn.position, shotSpawn.rotation);

		}

	}
}



