using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject followTarget;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float floatieness_start;  // floats... for floatieness
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
        floatieness = floatieness_start;
        float maxl = 0;
        zoomSpeed = zoomSpeed / 500;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        
        floatieness = floatieness_start * ((zoom / zoomMax) * (zoom / zoomMax));
        targetPosition = (followTarget.transform.position + offset);
        transform.position = transform.position + ((targetPosition - transform.forward * zoom) - transform.position) / (floatieness + 1);

        zoom += zoomSpeed * zoom * Input.GetAxis("Mouse ScrollWheel") * -1 * 60;
        zoom = Mathf.Clamp(zoom, 1, zoomMax);
        //transform.position = followTarget.transform.position + offset;
	}
}
