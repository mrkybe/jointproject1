using UnityEngine;
using System.Collections;

public class AttachUIRoot : MonoBehaviour {

    public Camera targetAttach;
	// Use this for initialization
	void Start ()
    {
        transform.parent = targetAttach.transform;
        //transform.rotation = targetAttach.transform.rotation;
        transform.position = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = Vector3.zero;
	}
}
