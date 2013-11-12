using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {

        transform.rigidbody.AddForce(new Vector3(Mathf.Sin(Time.time)*12.5f, 0, Mathf.Cos(Time.time)*Mathf.Cos(Time.time)*12.5f*12.5f));
	}
}
