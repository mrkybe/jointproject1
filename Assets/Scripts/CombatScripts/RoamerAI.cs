using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamerAI : MonoBehaviour {


	Vector3 startPosition;
	public float roamRadius = 3f;
	public float roamTimer = 3f;
	public float direactionChangeTime = 3f;
	private float latestDirectionChangeTime;
	private NavMeshAgent agent;

	Vector3 movement;

	public float speed = 5f;


	private float timer;
	void Awake()
	{
		latestDirectionChangeTime = 0f;
		agent = GetComponent<NavMeshAgent> ();

		timer = roamTimer;
		startPosition = transform.position;

	}
	void Update() 
	{
		if (Time.time - latestDirectionChangeTime > direactionChangeTime) {
			latestDirectionChangeTime = Time.time;
			newPosition ();

		}
		transform.position = new Vector3 ((movement.x * Time.deltaTime), transform.position.y,  (movement.z * Time.deltaTime));

	}
	public void newPosition() {
		Vector3 randDirection = Random.insideUnitSphere * roamRadius;
		randDirection.y = 0;
		randDirection = randDirection + startPosition;
		movement = randDirection * speed;
	}

}
