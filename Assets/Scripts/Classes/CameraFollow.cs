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
    private float zoomMax;
    [SerializeField]
    private float zoom;
    private Vector3 targetPosition;
    [SerializeField]
    private float zoomSpeed;
	// Use this for initialization
	void Start ()
    {
        zoomSpeed = zoomSpeed / 500;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        targetPosition = (followTarget.transform.position + offset);
        transform.position = transform.position + ((targetPosition - transform.forward * zoom) - transform.position) / (floatieness + 1);

        zoom += zoomSpeed * zoom * Input.GetAxis("Mouse ScrollWheel") * -1 * 60;
        zoom = Mathf.Clamp(zoom, 10, zoomMax);
        //transform.position = followTarget.transform.position + offset;
	}
}
