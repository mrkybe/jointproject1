using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Behavior_Designer.Runtime.Variables;
using Assets.Scripts.Classes.Libraries;
using Assets.Scripts.Classes.Mobile;
using Assets.Scripts.Classes.Static;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Scripts.Classes.Helper.Pilot
{
    /// <summary>
    /// Action to steer a ship towards a target position or Spaceship.
    /// </summary>
    class MoveToward : Action
    {
        public SharedVector3 Target;
        public SharedVector2 ControlStick;
        public SharedSpaceship TargetSpaceship;
        public SharedFloat TargetSpeed;
        public SharedFloat MaxSpeed = 3f;
        public SharedFloat AcceptableDistance = 1;
        public SharedFloat StopShort = 0f;
        public SharedBool SlowArrive = true;
        public SharedBool AvoidPlanets = false;
        public SharedFloat AvoidStrength = 0.5f;
        public SharedFloat AvoidDistance = 0.5f;

        private float stoppingDistance = 0f;
        private Rigidbody targetRigidbody = null;
        private Vector3 targetSpaceshipAcceleration = Vector3.zero;
        private Vector3 targetSpaceshipOldVelocity = Vector3.zero;
        private float targetSpaceshipOldVelocityTime = 0;
        private Rigidbody rigidbody;
        private Spaceship Shipscript;
        private bool arriving = false;

        public override void OnAwake()
        {
            base.OnAwake();
            Shipscript = GetComponent<Spaceship>();
            rigidbody = GetComponent<Rigidbody>();
            arriving = false;
        }

        public override void OnStart()
        {
            base.OnStart();
            rigidbody = GetComponent<Rigidbody>();
            Shipscript = GetComponent<Spaceship>();
        }

        public override void OnEnd()
        {
            targetRigidbody = null;
            stoppingDistance = 0f;
        }

        public override TaskStatus OnUpdate()
        {
            float stoppingDistanceNew = ((rigidbody.velocity.sqrMagnitude) /
                                         (2 * rigidbody.mass * Shipscript.EngineAcceleration * Time.fixedDeltaTime)) * 1.05f;
            stoppingDistance = (stoppingDistanceNew + stoppingDistance) / 2;

            if (TargetSpaceship.Value != null)
            {
                Target.Value = TargetSpaceship.Value.transform.position;
            }
            Vector3 targetPosition = Target.Value;
            if (StopShort.Value != 0)
            {
                Vector3 stoppingOffset = (this.transform.position - targetPosition).normalized * StopShort.Value;
                targetPosition += stoppingOffset;
            }
            Vector2 stick = new Vector2();
            float targetAngle = Vector3.Angle((transform.forward).normalized, (targetPosition - transform.position).normalized);
            if (isLeft(transform.position, transform.position + transform.forward * 500, targetPosition))
            {
                targetAngle = -targetAngle;
            }

            Vector3 temp = targetPosition - transform.position;
            
            stick = new Vector2(temp.x, temp.z).normalized;
            if (AvoidPlanets.Value)
            {
                var planets = Shipscript.GetInSensorRange<Planet>();
                if (planets.Count > 0)
                {
                    Vector3 pos1 = transform.position;
                    Vector3 pos2 = transform.position + transform.forward;
                    Vector3 pushVector = Vector3.zero;
                    foreach (var p in planets)
                    {
                        Vector3 ppos = Math3d.ProjectPointOnLine(pos1, pos2, p.transform.position);
                        int result = Math3d.PointOnWhichSideOfLineSegment(pos1, pos2, ppos);
                        float mul = 0;
                        if (result == 2 || result == 1)
                        {
                             mul = 1 - Mathf.Clamp01(Vector3.Distance(p.transform.position, transform.position) / (Shipscript.SensorRange * AvoidDistance.Value));
                        }
                        Vector3 dir = (p.transform.position - transform.position).normalized *  mul;
                        pushVector += dir;
                    }
                    pushVector /= planets.Count;
                    stick += new Vector2(pushVector.x, pushVector.z) * -AvoidStrength.Value;
                    stick = stick.normalized;
                }
            }

            Debug.DrawLine(transform.position, targetPosition, Color.white, BehaviorManager.instance.UpdateIntervalSeconds);
            Debug.DrawLine(transform.position, transform.position + transform.forward, Color.white, BehaviorManager.instance.UpdateIntervalSeconds);

            ControlStick.Value = stick;
            float dot = Mathf.Clamp(Vector3.Dot(temp, transform.forward), -1f, 1f);
            float d2 = Mathf.Max(dot, Shipscript.Grip);
            TargetSpeed.Value = Mathf.Clamp(d2 * MaxSpeed.Value, 0f, MaxSpeed.Value);

            if (SlowArrive.Value)
            {
                Vector3 proj = Math3d.ProjectPointOnLine(transform.position, transform.forward, targetPosition);
                int result = Math3d.PointOnWhichSideOfLineSegment(transform.position,
                    transform.position + transform.forward * stoppingDistance, proj); // Check if our target position is closer than our stopping distance.
                
                if (result == 0) // It is.
                {
                    TargetSpeed.Value = 0;
                }
                if (rigidbody.velocity.magnitude < 0.5f && Vector3.Magnitude(transform.position - Target.Value) < AcceptableDistance.Value)
                {
                    TargetSpeed.Value = 0;
                    Shipscript.GetPilot.TargetFaceDirection = transform.forward;
                    return TaskStatus.Success;
                }
                Debug.DrawLine(transform.position + transform.forward * stoppingDistance, transform.position + transform.forward * stoppingDistance + transform.up * 10f, Color.yellow);
            }
            else
            {
                if (Vector3.Magnitude(transform.position - Target.Value) < AcceptableDistance.Value)
                {
                    return TaskStatus.Success;
                }
            }

            return TaskStatus.Running;
        }

        /// <summary>
        /// Returns whether checkPoint is on the left side of the line defined by pos1 and pos2.
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <param name="checkPoint"></param>
        /// <returns></returns>
        private bool isLeft(Vector3 pos1, Vector3 pos2, Vector3 checkPoint)
        {
            return ((pos2.x - pos1.x) * (checkPoint.z - pos1.z) - (pos2.z - pos1.z) * (checkPoint.x - pos1.x)) > 0;
        }
    }
}
