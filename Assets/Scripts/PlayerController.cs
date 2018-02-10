using UnityEngine;

namespace Assets.Scripts {
    public class PlayerController : MonoBehaviour {

        public Rigidbody rb;
        public float speed;
        // Use this for initialization
        void Start () {
            rb = GetComponent<Rigidbody>();

        }
	
        // Update is called once per frame
        void Update () {
		
        }

        void FixedUpdate()  // called each physics steps
        {

            float moveHorizontal = Input.GetAxis ("Horizontal"); // default axis : Horizontal, vertical
            float moveVertical = Input.GetAxis ("Vertical");
            Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
            rb.velocity = movement*speed;


        }
    }
}
