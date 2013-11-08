using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject followTarget;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float floatieness;  // floats... for floatieness
    [SerializeField]
    private float zoom;
    private Vector3 targetPosition;
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        targetPosition = (followTarget.transform.position + offset);
        transform.position = transform.position + ((targetPosition + transform.forward * zoom) - transform.position) / (floatieness + 1);
        if(Input.GetKey(KeyCode.E))
        {
            zoom += 0.2f;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            zoom -= 0.2f;
        }
        zoom = Mathf.Clamp(zoom, -8, 8);
        //transform.position = followTarget.transform.position + offset;
	}
}
