using BehaviorDesigner.Runtime.Tasks.Basic.UnityTransform;
using UnityEngine;

namespace Assets
{
    public class CameraLookAt : MonoBehaviour
    {
        [SerializeField]
        private GameObject LookAtTarget;
        private Rigidbody rb;

        private Vector3 oldPos;

        [SerializeField] private Vector3 offset = Vector3.zero;
        // Use this for initialization
        void Start () {
            if (LookAtTarget == null)
            {
                LookAtTarget = GameObject.Find("PlayerShip");
                rb = LookAtTarget.GetComponent<Rigidbody>();
            }
        }
	
        // Update is called once per frame
        void FixedUpdate ()
        {
            if (rb == null)
            {
                rb = LookAtTarget.GetComponent<Rigidbody>();
            }
            else
            {
                Vector3 v = rb.velocity * 0.025f;
                float temp = v.z;
                v.z = -v.x;
                v.x = -temp;
                this.transform.LookAt(LookAtTarget.transform.position + v, Vector3.forward);
                Vector3 move = (LookAtTarget.transform.position + offset) - this.transform.position;
                this.transform.position += move * Time.deltaTime * 10.0f;
            }
        }
    }
}
