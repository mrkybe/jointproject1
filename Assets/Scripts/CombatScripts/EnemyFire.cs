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

	// The transform that the object is moving towards
	public SharedTransform target;

	public override void OnAwake ()
	{
		base.OnAwake ();
		shotSpawn = this.transform.GetChild (1).transform;

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
			
			nextFire.Value = Time.time + fireRate.Value;
			UnityEngine.GameObject.Instantiate(shot.Value, shotSpawn.position, shotSpawn.rotation);

		}

	}
}



