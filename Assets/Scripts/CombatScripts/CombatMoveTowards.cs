using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CombatMoveTowards : Action
{
	// The speed of the object
	public SharedFloat speed = 0; 
	// The transform that the object is moving towards
	public SharedTransform target;
	public SharedBool shouldFire = false;
	public SharedFloat fireRange = 0f;
	public SharedVector3 steeringDirection;


	public override void OnAwake ()
	{
//		Debug.Log (target);

	}


	public override TaskStatus OnUpdate()
	{
		// Return a task status of success once we've reached the target
		if (Vector3.SqrMagnitude(transform.position - target.Value.position) < fireRange.Value) {
			shouldFire.Value = true;
			//gameObject.transform.LookAt (target.Value);
			//Debug.Log("Close enough");
			return TaskStatus.Success;
		}
		// We haven't reached the target yet so keep moving towards it
		gameObject.transform.LookAt (target.Value);

		Vector3 playerDirection = target.Value.position - transform.position;
		Vector3 moveDirection = steeringDirection.Value + playerDirection.normalized * 10;
		Debug.DrawLine (transform.position, transform.position + moveDirection, Color.red);
		transform.position = Vector3.MoveTowards(transform.position,transform.position + moveDirection, speed.Value * Time.deltaTime * 0.5f);
		return TaskStatus.Running;
	}




}