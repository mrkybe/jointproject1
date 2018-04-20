using Assets.Scripts.Classes.Helper.ShipInternals;
using Assets.Scripts.Classes.Mobile;
using Assets.Scripts.Classes.WorldSingleton;
using UnityEngine;

namespace Assets.Scripts.Classes.Helper.Pilot {
    public abstract class PilotInterface : MonoBehaviour
    {
        public Faction Faction;
        protected Vector2 control_stickDirection;
        protected Vector3 targetFaceDirection;
        protected Vector3 targetVelocity;
        protected float throttle = 0f;
        public Person Identity;

        // Use this for initialization
        protected void Start()
        {
            control_stickDirection = new Vector2();
            targetFaceDirection = transform.forward;
            targetVelocity = Vector3.zero;
            throttle = 0f;
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

        public abstract void Die(Spaceship killer = null);

        public virtual void NotifyKilled(Spaceship victim, Spaceship killer = null)
        {
            // a winner is you!
        }

        public abstract void Pause();
        public abstract void Unpause();
    }
}
