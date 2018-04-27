using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFaceCamera : MonoBehaviour
{

    public Camera cameraToLookAt = null;

    // Use this for initialization
    void Start()
    {
        cameraToLookAt = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = cameraToLookAt.transform.position - transform.position;
        v.z = v.y = 0.0f;
        transform.LookAt(cameraToLookAt.transform.position - v);
        transform.Rotate(0, 180, 0);
        transform.localScale = new Vector3(1, 1, 1);
        if (this.transform.position.z < cameraToLookAt.transform.position.z)
        {
            transform.Rotate(0, 180, 0);
            transform.localScale = new Vector3(1, -1, 1);
        }
    }
}
