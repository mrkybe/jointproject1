using UnityEngine;

namespace Assets.Scripts.Classes {
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private GameObject followTarget;
        [SerializeField]
        private float offset;
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
            if (followTarget == null)
            {
                followTarget = GameObject.Find("PlayerShip");
            }
        }
	
        // Update is called once per frame
        void Update () 
        {
            if (followTarget != null)
            {
                floatieness = floatieness_start * ((zoom / zoomMax) * (zoom / zoomMax));
                targetPosition = (followTarget.transform.position + offset * transform.forward * -1);
                transform.position = transform.position + ((targetPosition - transform.forward * zoom) - transform.position) / (floatieness + 1);

                zoom += zoomSpeed * zoom * Input.GetAxis("Mouse ScrollWheel") * -1 * 60;
                zoom = Mathf.Clamp(zoom, 1, zoomMax);
                //transform.position = followTarget.transform.position + offset;
            }
        }
    }
}
