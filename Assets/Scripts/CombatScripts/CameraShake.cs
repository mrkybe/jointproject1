using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	private Vector3 camOriginalPos;
	public static float shakeAmount = 0.3f;
	public static float shakeDuration = 1f;
	private static CameraShake instance; 


	void Start () {
		camOriginalPos = gameObject.transform.localPosition;
		instance = this;
	}

	public static void Shake()
	{
		instance.StopAllCoroutines ();
		instance.StartCoroutine (instance.doShake ());
	}


	public IEnumerator doShake()
	{
		camOriginalPos = gameObject.transform.localPosition;
		float endTime = Time.time + shakeDuration;

		while (Time.time < endTime) 
		{
			transform.localPosition = camOriginalPos + Random.insideUnitSphere * shakeAmount;
			Debug.Log (camOriginalPos);
			Debug.Log (transform.localPosition);

			//shakeDuration -= Time.deltaTime;

			yield return null;
		}

		transform.localPosition = camOriginalPos;
	}


}
