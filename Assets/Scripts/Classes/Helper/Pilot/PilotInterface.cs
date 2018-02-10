using Assets.Scripts.Classes.Helper.ShipInternals;
using Assets.Scripts.Classes.Mobile;
using UnityEngine;

namespace Assets.Scripts.Classes.Helper.Pilot {
    public abstract class PilotInterface : MonoBehaviour
    {
        protected SensorArray mySensorArray;
        protected Vector2 control_stickDirection;
        protected Vector3 targetFaceDirection;
        protected Vector3 targetVelocity;
        protected float targetSpeed;
        protected float throttle = 0f;

        public SensorArray SensorArray
        {
            get { return mySensorArray; }
            set { mySensorArray = value; }
        }

        // Use this for initialization
        protected void Start()
        {
            control_stickDirection = new Vector2();
            targetFaceDirection = transform.forward;
            targetVelocity = Vector3.zero;
            throttle = 0f;
            targetSpeed = 0f;
        }

        protected void Update()
        {
            Debug.DrawLine(transform.position, transform.position + targetFaceDirection * 5f, Color.cyan);
        }

        public float Throttle
        {
            get { return Mathf.Clamp(throttle, -1f, 1f); }
        }

        public Vector3 TargetFaceDirection
        {
            get
            {
                return targetFaceDirection;
            }
            set { targetFaceDirection = value; }
        }

        public float TargetSpeed
        {
            get
            {
                //Debug.Log("Tried to get targetSpeed, gonna tell him " + targetSpeed);
                return targetSpeed;
            }
        }

        public abstract void Die();

        public virtual void NotifyKilled(Spaceship victim, Spaceship killer = null)
        {
            // a winner is you!
        }
    }
}
