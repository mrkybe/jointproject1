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

	public override TaskStatus OnUpdate()
	{
		// Return a task status of success once we've reached the target
		if (Vector3.SqrMagnitude(transform.position - target.Value.position) < 3f) {
			shouldFire.Value = true;
			//gameObject.transform.LookAt (target.Value);
			return TaskStatus.Success;
		}
		// We haven't reached the target yet so keep moving towards it
		gameObject.transform.LookAt (target.Value);
		transform.position = Vector3.MoveTowards(transform.position, target.Value.position, speed.Value * Time.deltaTime);
		return TaskStatus.Running;
	}
}