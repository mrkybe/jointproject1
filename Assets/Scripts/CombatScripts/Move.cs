using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<summary>
// The Move class is responsible for controlling how the player moves and the associated factors of speed and velocity.
// The Move class handles both controller input and keyboard/mouse input.
//</summary>
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
<<<<<<< HEAD
		rb = gameObject.GetComponent<Rigidbody>();
=======
        rb = gameObject.GetComponent<Rigidbody>();
>>>>>>> master
    }

	void Update()
	{
		timeMove ();
<<<<<<< HEAD
		//checkCont ();
=======
>>>>>>> master
	}
	// Update is called once per frame
	void FixedUpdate ()
    {
<<<<<<< HEAD
		//forceMove ();
		rb.velocity = velocity;
        look();
=======
		
        look();
		rb.velocity = velocity;
>>>>>>> master
    }

	//<summary>
	// timeMove function is movement based on time. Get the x and z axis input (We are top-down) from the left stick to move the player in those directions.
	// Add the velocity to the players velocity; this function is called every frame.
	//</summary>
	void timeMove()
	{
		float x = Input.GetAxisRaw("Horizontal");
		float z = Input.GetAxisRaw("Vertical");

		Vector3 move = new Vector3 (x, 0, z);

		//transform.position += move * speed / Time.deltaTime;
		/*
		float rx = Input.GetAxis("HorizontalR");
		float rz = Input.GetAxis("VerticalR");

		float angle = Mathf.Atan2 (rx, rz) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.EulerAngles (0,angle * rotateSpeed,0);
<<<<<<< HEAD
        */
		velocity = move * speed;
	}
		

=======
        velocity = move * speed;
	}
	//<summary>
	// look function handles both mouse and controller inputs. Using the mouse, generate a ray at the mouse screen position and rotate the player
	// to face that mouse position. For the controller, get the axis values from the right stick and calculate a vector3 direction. Rotate the player
	// to face that vector3 direction.
	//</summary>
>>>>>>> master
    void look()
    {
        
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
