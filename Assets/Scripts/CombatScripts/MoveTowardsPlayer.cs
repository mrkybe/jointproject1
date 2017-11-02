using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour {

	private Transform target;
	public float speed = 5f;

	void Start () {
		target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update () {
		if (target == null) return;
		Vector3 targetPosition = new Vector3(target.position.x, target.position.y, target.position.z);
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
	}


}