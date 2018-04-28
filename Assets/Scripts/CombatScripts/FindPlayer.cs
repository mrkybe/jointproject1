using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FindPlayer : Conditional
{
	// How wide of an angle the object can see
	public SharedFloat fieldOfViewAngle;

	public string targetTag = "Player";

	public SharedTransform target ;



	//private Transform targetTransform;

	private Transform[] possibleTargets;

	public override void OnAwake()
	{

		var targets = GameObject.FindGameObjectsWithTag(targetTag);
		possibleTargets = new Transform[targets.Length];
		for (int i = 0; i < targets.Length; ++i) {
			possibleTargets[i] = targets[i].transform;
		}
	}

	public override TaskStatus OnUpdate()
	{
		// Return success if a target is within sight
		for (int i = 0; i < possibleTargets.Length; ++i) {
			if (FoundPlayer(possibleTargets[i], fieldOfViewAngle.Value)) {
				
				target.Value = possibleTargets[i];
				return TaskStatus.Success;
			}
		}
		return TaskStatus.Failure;
	}

	// Returns true if targetTransform is within sight of current transform
	public bool FoundPlayer(Transform targetTransform, SharedFloat fieldOfViewAngle)
	{
		Vector3 direction = targetTransform.position - transform.position;
		bool result = false;

		bool inDistance = false; 
		if (Vector3.SqrMagnitude (transform.position - target.Value.position) < 300f) {
			inDistance = true; 
		}

		bool inAngle = false; 
		if (Vector3.Angle (direction, transform.forward) < fieldOfViewAngle.Value) {
			inAngle = true;
		}
		result = inAngle & inDistance;

		return result;
	}
}