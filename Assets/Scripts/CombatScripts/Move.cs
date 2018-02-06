using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public float speed = 10f;
	public float rotateSpeed = .05f;
    public bool controller;

    private Rigidbody rb;
	private Vector3 lookPos;
    private Vector3 velocity;
    // Use this for initialization
    void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

	void Update()
	{
		timeMove ();
	}
	// Update is called once per frame
	void FixedUpdate ()
    {
		
        look();
		rb.velocity = velocity;
    }


	//movement based on time
	void timeMove()
	{
		float x = Input.GetAxisRaw("Horizontal");
		float z = Input.GetAxisRaw("Vertical");

		Vector3 move = new Vector3 (x, 0, z);

		float rx = Input.GetAxis("HorizontalR");
		float rz = Input.GetAxis("VerticalR");

		float angle = Mathf.Atan2 (rx, rz) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.EulerAngles (0,angle * rotateSpeed,0);
        velocity = move * speed;
	}
    void look()
    {
        // if there is no controller, follow the mouse
        // else if there is a controller, use the sticks to
        // calculate direction
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float raylength;

        if (ground.Raycast(ray, out raylength))
        {
            Vector3 pointToLook = ray.GetPoint(raylength);
            Debug.DrawLine(ray.origin, pointToLook, Color.magenta);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
        float rx = Input.GetAxis("HorizontalR");
        float rz = Input.GetAxis("VerticalR");
        Vector3 direction = Vector3.right * rx + Vector3.forward * rz;
        if (direction.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
		
}
