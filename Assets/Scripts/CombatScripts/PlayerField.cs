using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class PlayerField : Action
{

	private SharedFloat speed = 15f; 
	public SharedTransform Player;
	private SharedVector3 des;


	public override void OnAwake ()
	{
		Vector3 playerPos = Player.Value.position;
		des = new Vector3(Random.Range(playerPos.x-300, playerPos.x +300), 0, Random.Range(playerPos.z-300, playerPos.z+300));

	}
		
	public override TaskStatus OnUpdate()
	{

		if (Vector3.SqrMagnitude (transform.position - Player.Value.position) < 300f) {


			return TaskStatus.Success;
		}
		transform.position = Vector3.MoveTowards (transform.position, des.Value, speed.Value * Time.deltaTime);
		return TaskStatus.Running;
	}
}

