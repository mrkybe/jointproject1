using System;
using System.Collections.Generic;
using Assets.Behavior_Designer.Runtime.Variables;
using Assets.Scripts.Classes.Mobile;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.WorldSingleton;
using BehaviorDesigner.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Classes.Helper.Pilot {
    /// <summary>
    /// The AI for the ships/fleets on the Overmap.  Executes a Behavior Tree.  Provides methods for changing it.
    /// </summary>
    public class AI_Patrol : PilotInterface
    {
        public ExternalBehaviorTree ExternalMiningBehaviorTree;
        public ExternalBehaviorTree ExternalDeliveryBehaviorTree;
        public ExternalBehaviorTree ExternalPirateBehaviorTree;
        public ExternalBehaviorTree ExternalScrapperBehaviorTree;

        private SharedBool Alive;
        private SharedBool Afraid;
        private SharedInt Bravery;
        private SharedBool FreshKill;
        private SharedBool Safe;
        private SharedVector2 ControlStick;
        private SharedPlanet HomePlanet;
        private SharedSpaceship shipScript;
        private SharedSpaceship AttackTarget;
        private SharedSpaceship AttackTargetMIA;
        private SharedFloat CruiseSpeed;
        private SharedFloat EmergencySpeed;

        public SharedFloat TargetSpeed;

        private BehaviorTree behaviorTree;
        private float interactionDistance = 5f;
        private new Rigidbody rigidbody;
        public float Speed;

        public int Money = 0;

        // Use this for initialization
        public void Awake()
        {
            behaviorTree = transform.GetComponent<BehaviorTree>();
            if (!behaviorTree)
            {
                behaviorTree = gameObject.AddComponent<BehaviorTree>();
                behaviorTree.ExternalBehavior = ExternalMiningBehaviorTree;
                behaviorTree.StartWhenEnabled = true;
            }
        }

        private void InitializeBehaviorTreeVariableReferences()
        {
            rigidbody = GetComponent<Rigidbody>();

            Alive = (SharedBool)behaviorTree.GetVariable("Alive");
            Afraid = (SharedBool)behaviorTree.GetVariable("Afraid");
            Bravery = (SharedInt)behaviorTree.GetVariable("Bravery");
            ControlStick = (SharedVector2)behaviorTree.GetVariable("ControlStick");
            TargetSpeed = (SharedFloat)behaviorTree.GetVariable("TargetSpeed");
            HomePlanet = (SharedPlanet)behaviorTree.GetVariable("HomePlanet");
            shipScript = (SharedSpaceship)behaviorTree.GetVariable("Shipscript");
            CruiseSpeed = (SharedFloat)behaviorTree.GetVariable("CruiseSpeed");
            EmergencySpeed = (SharedFloat)behaviorTree.GetVariable("EmergencySpeed");

            shipScript.Value = transform.GetComponent<Spaceship>();
        }

        public new void Start()
        {
            base.Start();
            InitializeBehaviorTreeVariableReferences();
            if (Identity == null)
            {
                Identity = NameGenerator.Main.RandomPerson();
            }
        }

        public new void Update()
        {
            base.Update();
            Speed = rigidbody.velocity.magnitude;
            targetFaceDirection = new Vector3(ControlStick.Value.x, 0, ControlStick.Value.y);
            if (ControlStick.Value.sqrMagnitude <= 0.001)
            {
                targetFaceDirection = transform.forward;
            }

            float ratio = Mathf.Clamp((TargetSpeed.Value - Speed), -1.0f, 1.0f);
            if (float.IsNaN(ratio))
            {
                ratio = 0;
            }
            throttle = (ratio) * shipScript.Value.EngineAcceleration;
        }

        /// <summary>
        /// Returns the Shipscript that I am the pilot of.
        /// </summary>
        /// <returns></returns>
        public Spaceship GetShip()
        {
            return shipScript.Value;
        }

        /// <summary>
        /// Returns the Behavior Tree script that makes my decisions.
        /// </summary>
        /// <returns></returns>
        public BehaviorTree GetBehaviorTree()
        {
            return behaviorTree;
        }

        private void StartBehaviorTree()
        {
            CruiseSpeed.Value = shipScript.Value.MaxSpeed / 2f;
            EmergencySpeed.Value = shipScript.Value.MaxSpeed / 1.1f;
            behaviorTree.Start();
        }

        /// <summary>
        /// Sets the Behavior Tree the one for mining.
        /// </summary>
        /// <param name="miningTargets">The kind of resources to mine.</param>
        /// <param name="homePlanet">The planet we drop off resources at.</param>
        public void StartMining(List<string> miningTargets, Planet homePlanet)
        {
            behaviorTree.ExternalBehavior = ExternalMiningBehaviorTree;
            InitializeBehaviorTreeVariableReferences();

            HomePlanet.Value = homePlanet;
            behaviorTree.GetVariable("MiningTargets").SetValue(miningTargets);
            shipScript.Value.EngineAcceleration = 25f + Random.value * 25f;
            shipScript.Value.MaxSpeed = 3f + Random.value * 2.5f;

            ModelSwitcher modelSwitcher = shipScript.Value.GetComponentInChildren<ModelSwitcher>();
            if (modelSwitcher)
            {
                int[] choices = { 10 };
                int modelChoice = (int)(Random.value * choices.Length);
                Debug.Log(choices[modelChoice]);
                modelSwitcher.SetModel(choices[modelChoice], true);
            }

            StartBehaviorTree();
        }

        /// <summary>
        /// Sets the Behavior Tree the one for delivering an order.
        /// </summary>
        /// <param name="order">The order that the ship is responsible for completing.</param>
        /// <param name="randomModel">Choose a new random model/mesh for this ship?.</param>
        public void StartDelivery(MarketOrder order, bool randomModel = false)
        {
            behaviorTree.ExternalBehavior = ExternalDeliveryBehaviorTree;
            InitializeBehaviorTreeVariableReferences();

            HomePlanet.Value = order.origin;
            behaviorTree.GetVariable("DeliveryOrder").SetValue(order);
            behaviorTree.GetVariable("DeliveryPlanet").SetValue(order.destination);
            shipScript.Value.EngineAcceleration = 25f + Random.value * 25f;
            shipScript.Value.MaxSpeed = 5f + Random.value * 2.5f;
            shipScript.Value.PowerLevel = 5 + (int)(Random.value * 5);
            order.ship = shipScript.Value;

            if (randomModel)
            {
                ModelSwitcher modelSwitcher = shipScript.Value.GetComponentInChildren<ModelSwitcher>();
                if (modelSwitcher)
                {
                    int[] choices = {0, 1, 2, 3, 6, 7, 8, 9};
                    int modelChoice = (int)(Random.value * choices.Length);
                    modelSwitcher.SetModel(choices[modelChoice], true);
                }
            }

            StartBehaviorTree();
        }

        /// <summary>
        /// Sets the Behavior Tree to the one for piracy.
        /// </summary>
        public void StartPirate()
        {
            behaviorTree.ExternalBehavior = ExternalPirateBehaviorTree;
            InitializeBehaviorTreeVariableReferences();

            AttackTarget = (SharedSpaceship)behaviorTree.GetVariable("AttackTarget");
            AttackTargetMIA = (SharedSpaceship)behaviorTree.GetVariable("AttackTargetMIA");
            FreshKill = (SharedBool)behaviorTree.GetVariable("FreshKill");

            AttackTarget.Value = null;
            AttackTargetMIA.Value = null;
            FreshKill.Value = false;
            
            shipScript.Value.EngineAcceleration = 150f + Random.value * 300f;
            shipScript.Value.MaxSpeed = 4f + Random.value * 3f;
            shipScript.Value.TurningSpeed = 0.8f + Random.value;
            shipScript.Value.PowerLevel = 7 + (int) (Random.value * 7);

            ModelSwitcher modelSwitcher = shipScript.Value.GetComponentInChildren<ModelSwitcher>();
            if (modelSwitcher)
            {
                int[] choices = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 11};
                int modelChoice = (int)(Random.value * choices.Length);
                //Debug.Log(choices[modelChoice]);
                modelSwitcher.SetModel(choices[modelChoice], true);
            }

            StartBehaviorTree();
        }

        /// <summary>
        /// Sets the Behavior Tree to the one for scrapping.
        /// </summary>
        public void StartScrapper()
        {
            behaviorTree.ExternalBehavior = ExternalScrapperBehaviorTree;
            InitializeBehaviorTreeVariableReferences();

            Faction = Overseer.Main.GetFaction("Robots");
            AttackTarget = (SharedSpaceship)behaviorTree.GetVariable("AttackTarget");
            AttackTarget.Value = null;

            ModelSwitcher modelSwitcher = shipScript.Value.GetComponentInChildren<ModelSwitcher>();
            if (modelSwitcher)
            {
                int[] choices = { 5 };
                int modelChoice = (int)(Random.value * choices.Length);
                modelSwitcher.SetModel(choices[modelChoice], true);
            }
            StartBehaviorTree();
        }

        /// <summary>
        /// This kills the Pilot.  Sets a Flag in the Blackboard for being dead,
        /// Behavior Trees must clean up after themselves and go into their dead state.
        /// </summary>
        public override void Die(Spaceship killer = null)
        {
            Alive.SetValue(false);

            // Tell our faction to increase the bounty on our killer
            Faction.NotifyOfAttack(shipScript, killer);

            // FOR DELIVERY SHIPS, PROPAGATE ORDER FAILURE
            SharedMarketOrder myOrder = (SharedMarketOrder)behaviorTree.GetVariable("DeliveryOrder");
            if (myOrder != null && myOrder.Value != null)
            {
                if (myOrder.Value.GetOrderStatus() == MarketOrder.OrderStatus.IN_PROGRESS)
                {
                    myOrder.Value.Fail(MarketOrder.OrderStatus.FAIL_SHIP_DEAD);
                }
            }
        }

        /// <summary>
        /// Called when a ship that is in sensor range is killed, so that we know its dead.
        /// </summary>
        /// <param name="victim"></param>
        /// <param name="killer"></param>
        public override void NotifyKilled(Spaceship victim, Spaceship killer = null)
        {
            base.NotifyKilled(victim, killer);
            if (AttackTarget != null && AttackTarget.Value == victim)
            {
                if (killer == shipScript.Value)
                {
                    FreshKill.Value = true;
                    // Since we killed it, try to claim the bounty on it

                    Money += Overseer.Main.ClaimBountyOn(victim);
                }
            }
        }

        public override void Pause()
        {
            behaviorTree.PauseWhenDisabled = true;
            behaviorTree.DisableBehavior(true);
        }

        public override void Unpause()
        {
            behaviorTree.PauseWhenDisabled = false;
            behaviorTree.EnableBehavior();
        }

        /// <summary>
        /// Called when a new ship enters our sensor range.
        /// </summary>
        /// <param name="contact"></param>
        public void NotifyShip(Spaceship contact)
        {
            // FOR SCRAPPER //
            if (AttackTarget != null && AttackTarget.Value == null)
            {
                if (!contact.Alive)
                {
                    AttackTarget.Value = contact;
                }
            }

            // FOR PIRATES  //
            // If we find the ship we were just chasing, set it as our target again.
            if (AttackTargetMIA != null && AttackTargetMIA.Value == contact)
            {
                if (contact.Alive)
                {
                    AttackTarget.Value = contact;
                    AttackTargetMIA.Value = null;
                }
            }

            // Basically, if we're a pirate, check whether we're hunting for our next victim and set this ship to be our new target if we are.
            if (AttackTarget != null && AttackTarget.Value == null && AttackTargetMIA != null && AttackTargetMIA.Value == null && FreshKill != null)
            {
                try
                {
                    bool scary = contact.GetScaryness(shipScript.Value) < Bravery.Value;
                    bool hostile = contact.Pilot.Faction.HostileWith(Faction);
                    if (contact.Alive && scary && !FreshKill.Value && hostile)
                    {
                        AttackTarget.Value = contact;
                    }
                }
                catch (Exception ex)
                {
                    bool scary = contact.GetScaryness(shipScript.Value) < Bravery.Value;
                    bool hostile = contact.Pilot.Faction.HostileWith(Faction);
                    if (contact.Alive && scary && !FreshKill.Value && hostile)
                    {
                        AttackTarget.Value = contact;
                    }
                }
            }
        }

        public void NotifyShipMissing(Spaceship contact)
        {
            if (AttackTarget != null && AttackTargetMIA != null && contact == AttackTarget.Value)
            {
                AttackTargetMIA.Value = contact;
                AttackTarget.Value = null;
            }
        }
    }
}
