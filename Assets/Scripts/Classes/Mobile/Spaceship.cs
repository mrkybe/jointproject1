using System;
using System.Collections.Generic;
using Assets.Scripts.Classes.Helper;
using Assets.Scripts.Classes.Helper.Pilot;
using Assets.Scripts.Classes.Helper.ShipInternals;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.WorldSingleton;
using UnityEngine;

namespace Assets.Scripts.Classes.Mobile {
    /// <summary>
    /// The physical Spaceship.  Keeps track of 'physical' information, moves it every update.  Requires a Pilot, human or AI.
    /// </summary>
    [SelectionBase]
    public class Spaceship : MonoBehaviour
    {
        [SerializeField]
        private float engineAcceleration;
        [SerializeField]
        private float maxSpeed;
        [SerializeField]
        private float turningSpeed;

        [SerializeField]
        [Range(0,1)]
        private float manuverability;
        [SerializeField]
        private AI_Type desired_AI_Type;
        [SerializeField]
        public int PowerLevel;
        [SerializeField]
        public int HullHealth;
        [SerializeField]
        public float InteractionRange = 5f;
        [SerializeField]
        public float NotificationRange = 50f;
        [SerializeField]
        private float sensorRange;
        [SerializeField]
        public LayerMask NotificationLayerMask;

        public float EngineForce = 0;

        [SerializeField]
        public bool CheatSpeed = false;

        private float targetSpeed;
        private float throttle_input;
        private float old_throttleInput;
        private float grip = 0;
        private CargoHold myStorage;
        private ModelSwitcher myModelSwitcher;
        // Use this for initialization

        protected Vector3 direction;
        protected float velocity;
        [SerializeField]
        protected bool isAI;
        [SerializeField]
        protected bool isAffectedByGravity;
        protected PilotInterface pilot;
        private Vector3 gravityVector;

        public Rigidbody MyRigidbody;
        // Use this for initialization

        [SerializeField]
        public List<GameObject> inSensorRange = new List<GameObject>();

        private int modelChoice = 0;
        private void Awake()
        {
            pilot = GetComponent<PilotInterface>();
            BlackBox = new BlackBox();

            targetSpeed = -999;
            throttle_input = 0;
            old_throttleInput = 0;
            PowerLevel = 10;
            HullHealth = 100;
            InteractionRange = 8f;
            Alive = true;

            if (isAI)
            {
                targetSpeed = 0;
            }
            myStorage = new CargoHold(this, 100);
        }

        private void Start ()
        {
            MyRigidbody = GetComponent<Rigidbody>();
            myModelSwitcher = GetComponentInChildren<ModelSwitcher>();
            myModelSwitcher.SetSensorRange(SensorRange);
            if (Overseer.Main.IsOvermapPaused())
            {
                Pause();
            }
        }


        private Vector3 savedVelocity = Vector3.zero;
        private Vector3 savedAngularVelocity = Vector3.zero;
        public void Pause()
        {
            savedVelocity = MyRigidbody.velocity;
            savedAngularVelocity = MyRigidbody.angularVelocity;
            MyRigidbody.freezeRotation = true;
            MyRigidbody.velocity = Vector3.zero;
            MyRigidbody.angularVelocity = Vector3.zero;
            if (pilot)
            {
                pilot.Pause();
            }
        }

        public void Unpause()
        {
            MyRigidbody.freezeRotation = false;
            MyRigidbody.velocity = savedVelocity;
            MyRigidbody.angularVelocity = savedAngularVelocity;
            if (pilot)
            {
                pilot.Unpause();
            }
        }

        // Update is called once per frame
        private Vector3 dAngularVelocity = Vector3.zero;
        private Vector3 dVelocity = Vector3.zero;
        private float dTorquey = 0;
        private float torquey = 0;

        private void UpdateDerivatives()
        {
            torquey = Vector3.SignedAngle(transform.forward, pilot.TargetFaceDirection, Vector3.up);
            dTorquey = (torquey - old_torquey) / Time.fixedDeltaTime;

            dVelocity = (MyRigidbody.velocity - old_velocity) / Time.fixedDeltaTime;
            dAngularVelocity = (MyRigidbody.angularVelocity - old_angularVelocity) / Time.fixedDeltaTime;
        }

        private void Update ()
        {
            if (!Overseer.Main.IsOvermapPaused() && pilot)
            {
                UpdateDerivatives();

                direction = transform.forward;
                //velocity = engineRunSpeed;

                float av = MyRigidbody.angularVelocity.y;
                float max_torq = TurningSpeed;
                float nT = (dTorquey / 2f);
                float torq = Mathf.Clamp(torquey + nT, -max_torq - av, max_torq - av);
                MyRigidbody.AddTorque(new Vector3(0, torq, 0));
                Move();
            }
        }

        private Vector3 old_angularVelocity = Vector3.zero;
        private Vector3 old_velocity = Vector3.zero;
        private float old_torquey = 0;

        public void LateUpdate()
        {
            old_throttleInput = throttle_input;
            old_angularVelocity = MyRigidbody.angularVelocity;
            old_velocity = MyRigidbody.velocity;
            old_torquey = torquey;
        }

        private void Move()
        {
            Vector3 engineVel = (direction.normalized * pilot.Throttle);
            Vector3 finalVel = (engineVel) * Time.deltaTime * engineAcceleration;
            float velmax = Mathf.Min(Manuverability, (MyRigidbody.velocity.magnitude) / maxSpeed);
            Grip = velmax;
            MyRigidbody.AddForce(finalVel);
            float dot = Vector3.Dot(MyRigidbody.velocity.normalized, MyRigidbody.transform.forward);
            MyRigidbody.velocity = MyRigidbody.velocity * Mathf.Clamp(velmax, 0.0f, 1f) + MyRigidbody.transform.forward * MyRigidbody.velocity.magnitude * Mathf.Clamp(1.0f-velmax, 0, 1.0f) * (((1+dot)/2));
        }
        
        /// <summary>
        /// Unregisters a collider that entered the ship's sensor range.
        /// </summary>
        /// <param name="other"></param>
        public void SensorEnter(Collider other)
        {
            // New entity in sensor range.
            inSensorRange.Add(other.gameObject.transform.root.gameObject);
            if (pilot.GetType() == typeof(AI_Patrol)) // If we're an AI ship...
            {
                Spaceship contact = other.GetComponent<Spaceship>();
                if (contact)
                {
                    ((AI_Patrol)pilot).NotifyShip(contact);
                }
            }
        }

        /// <summary>
        /// Registers a collider that enters the ship's sensor range.
        /// </summary>
        /// <param name="other"></param>
        public void SensorExit(Collider other)
        {
            // Entity leaves sensor range.
            inSensorRange.Remove(other.gameObject.transform.root.gameObject);
            if (pilot.GetType() == typeof(AI_Patrol)) // If we're an AI ship...
            {
                Spaceship contact = other.GetComponent<Spaceship>();
                if (contact)
                {
                    ((AI_Patrol)pilot).NotifyShipMissing(contact);
                }
            }
        }
        
        private void SensorListRemoveNulls()
        {
            inSensorRange.RemoveAll(x => x == null);
        }

        /// <summary>
        /// Returns a list of specified entities in sensor range.
        /// </summary>
        public List<T> GetInSensorRange<T>()
        {
            List<T> targets = new List<T>();
            SensorListRemoveNulls(); // Entities aren't removed from the list if they are destroyed, so delete nulls first.
            for (int i = 0; i < inSensorRange.Count; i++)
            {
                T target = inSensorRange[i].GetComponent<T>();
                if (target != null)
                {
                    targets.Add(target);
                }
            }
            return targets;
        }

        /// <summary>
        /// Returns a list of specified entities in interaction range.
        /// </summary>
        public List<T> GetInInteractionRange<T>()
        {
            List<T> targets = new List<T>();
            SensorListRemoveNulls(); // Entities aren't removed from the list if they are destroyed, so delete nulls first.
            for (int i = 0; i < inSensorRange.Count; i++)
            {
                if (inSensorRange[i] != null)
                {
                    T target = inSensorRange[i].GetComponent<T>();
                    if (target != null && Vector3.Distance(transform.position, inSensorRange[i].transform.root.position) < InteractionRange)
                    {
                        targets.Add(target);
                    }
                }
            }
            return targets;
        }
        /// <summary>
        /// Returns the sensor range of the ship.
        /// </summary>
        public float SensorRange
        {
            get { return sensorRange; }
            set
            {
                sensorRange = value;
                myModelSwitcher.SetSensorRange(value);
            }
        }

        /// <summary>
        /// Returns how acceleration of the ship.
        /// </summary>
        public float EngineAcceleration
        {
            get { return engineAcceleration; }
            set { engineAcceleration = value; }
        }

        /// <summary>
        /// Returns the max speed of the ship.
        /// </summary>
        public float MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }

        /// <summary>
        /// Returns the turning speed of the ship.
        /// </summary>
        public float TurningSpeed
        {
            get { return turningSpeed; }
            set { turningSpeed = value; }
        }
        
        /// <summary>
        /// Returns the manuverability of the ship.
        /// </summary>
        public float Manuverability
        {
            get { return manuverability; }
            set { manuverability = value; }
        }
        
        /// <summary>
        /// Returns the speed the ship is trying to reach.
        /// </summary>
        public float TargetSpeed
        {
            get { return targetSpeed; }
            set { targetSpeed = value; }
        }

        /// <summary>
        /// Returns the amount of grip the ship has.
        /// </summary>
        public float Grip
        {
            get { return grip; }
            private set { grip = value; }
        }

        /// <summary>
        /// Returns the Throttle input by the pilot.
        /// </summary>
        public float ThrottleInput
        {
            get
            {
                if (pilot)
                {
                    return pilot.Throttle;
                }
                else return 0;
            }
        }
        
        /// <summary>
        /// Returns the ship's Cargohold.
        /// </summary>
        public CargoHold GetCargoHold
        {
            get { return myStorage; }
        }

        /// <summary>
        /// Returns the ship's pilot.
        /// </summary>
        public PilotInterface Pilot
        {
            get { return pilot; }
        }

        /// <summary>
        /// Is the ship alive?
        /// </summary>
        public bool Alive
        {
            get; internal set;
        }

        public BlackBox BlackBox
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns how scary another ship is compared to mine.
        /// </summary>
        /// <param name="other">The source of the scaryness.</param>
        /// <returns></returns>
        public int GetScaryness(Spaceship other)
        {
            return PowerLevel - other.PowerLevel;
        }

        /// <summary>
        /// Deals damage to the ship, dies upon reaching 0 health.
        /// </summary>
        /// <param name="damage">Amount of damage, (postive integer).</param>
        /// <param name="source">Source of damage.</param>
        public void TakeDamage(int damage, Spaceship source = null)
        {
            HullHealth -= damage;
            if (HullHealth <= 0)
            {
                if (source != null)
                {
                    Die(source);
                }
                else
                {
                    Die();
                }
            }
        }


        private void Die(Spaceship killer = null)
        {
            // Tell every ship that has us on sensors that we died.
            Collider[] f = Physics.OverlapSphere(transform.position, NotificationRange, NotificationLayerMask, QueryTriggerInteraction.Collide);
            foreach(Collider c in f)
            {
                AI_Patrol comms = c.gameObject.GetComponent<AI_Patrol>();
                if (comms)
                {
                    comms.NotifyKilled(this, killer);
                }
            }
            
            // Change apperance to destroyed.
            myModelSwitcher.BecomeGraveyard();
            Alive = false;
            Pilot.Die(killer);
        }

        public void DeleteSelf()
        {
            Destroy(gameObject, 2);
        }

        private void CalculateGravityVector()
        {
            //Debug.Log("GRAVITY VECTOR " + Planet.listOfPlanetObjects.Count);
            gravityVector = Vector3.zero;

            foreach (var x in Planet.listOfPlanetObjects)
            {
                //List<Vector3> planetPositions = new List<Vector3>();
                //List<double> planetMasses = new List<double>();

                if (x.hasGravity)
                {
                    Vector3 offset = x.transform.position - transform.position;
                    double g = x.Mass / offset.sqrMagnitude;
                    if (offset.magnitude < (x.Radius + 1))
                    {
                        g = 0;
                    }
                    Vector3 norm = offset.normalized;
                    norm.Scale(new Vector3((float)g, 0, (float)g));
                    gravityVector = gravityVector + norm;
                    //Debug.Log(g);
                    //planetPositions.Add(offset);
                    //planetMasses.Add(x.MassKilotons);
                }
            }
        }
    }
}