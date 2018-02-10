using System.Collections.Generic;
using Assets.Behavior_Designer.Runtime.Variables;
using Assets.Scripts.Classes.Mobile;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.WorldSingleton;
using BehaviorDesigner.Runtime;
using UnityEngine;

/* AI_Patrol is the AI for the ships/fleets on the Overmap.
 * It contains all of the atomic AI methods that are used by the AI behavior tree.
 * It provides public functions for Factions/Planets to give orders through.
 * These orders are fulfilled by replacing the current BehaviorTree with a new one.
 */

namespace Assets.Scripts.Classes.Helper.Pilot {
    public class AI_Patrol : PilotInterface
    {
        public ExternalBehaviorTree ExternalMiningBehaviorTree;
        public ExternalBehaviorTree ExternalDeliveryBehaviorTree;
        public ExternalBehaviorTree ExternalPirateBehaviorTree;

        private SharedBool Alive;
        private SharedBool HasVictim;
        private SharedBool FreshKill;
        private SharedBool Safe;
        private SharedVector2 ControlStick;
        private SharedPlanet HomePlanet;
        private SharedSpaceship shipScript;
        private SharedSpaceship AttackTarget;

        private BehaviorTree behaviorTree;
        private float interactionDistance = 5f;
        private Rigidbody rigidbody;
        public float Speed;
        public SharedFloat TargetSpeed;

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
            ControlStick = (SharedVector2)behaviorTree.GetVariable("ControlStick");
            TargetSpeed = (SharedFloat)behaviorTree.GetVariable("TargetSpeed");
            HomePlanet = (SharedPlanet)behaviorTree.GetVariable("HomePlanet");
            shipScript = (SharedSpaceship)behaviorTree.GetVariable("Shipscript");

            shipScript.Value = transform.GetComponent<Spaceship>();
        }

        public new void Start()
        {
            base.Start();
            InitializeBehaviorTreeVariableReferences();
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

            behaviorTree.Start();
        }

        /// <summary>
        /// Sets the Behavior Tree the one for delivering an order.
        /// </summary>
        /// <param name="order">The order that the ship is responsible for completing.</param>
        public void StartDelivery(MarketOrder order)
        {
            behaviorTree.ExternalBehavior = ExternalDeliveryBehaviorTree;
            InitializeBehaviorTreeVariableReferences();

            HomePlanet.Value = order.origin;
            behaviorTree.GetVariable("DeliveryOrder").SetValue(order);
            behaviorTree.GetVariable("DeliveryPlanet").SetValue(order.destination);
            behaviorTree.Start();
        }

        /// <summary>
        /// Sets the Behavior Tree to the one for piracy.
        /// </summary>
        public void StartPirate()
        {
            behaviorTree.ExternalBehavior = ExternalPirateBehaviorTree;
            InitializeBehaviorTreeVariableReferences();

            AttackTarget = (SharedSpaceship)behaviorTree.GetVariable("AttackTarget");
            HasVictim = (SharedBool)behaviorTree.GetVariable("HasVictim");
            FreshKill = (SharedBool)behaviorTree.GetVariable("FreshKill");

            AttackTarget.Value = null;
            HasVictim.Value = false;
            FreshKill.Value = false;

            shipScript.Value.Faction = Overseer.Main.GetFaction("Pirates");
            behaviorTree.Start();
        }

        /// <summary>
        /// This kills the Pilot.  Sets a Flag in the Blackboard for being dead,
        /// Behavior Trees must clean up after themselves and go into their dead state.
        /// </summary>
        public override void Die()
        {
            Alive.SetValue(false);
        }

        public override void NotifyKilled(Spaceship victim, Spaceship killer = null)
        {
            base.NotifyKilled(victim, killer);
            if (AttackTarget != null && HasVictim != null && AttackTarget.Value == victim)
            {
                AttackTarget.Value = null;
                HasVictim.Value = false;
                if (killer == shipScript.Value)
                {
                    FreshKill.Value = true;
                }
            }
        }

        public void NotifyShip(Spaceship contact)
        {
            if (HasVictim != null && AttackTarget != null && FreshKill != null)
            {
                if (contact.GetScaryness(shipScript.Value) < 0 && !HasVictim.Value && !FreshKill.Value)
                {
                    HasVictim.Value = true;
                    AttackTarget.Value = contact;
                }
            }
        }
    }
}
