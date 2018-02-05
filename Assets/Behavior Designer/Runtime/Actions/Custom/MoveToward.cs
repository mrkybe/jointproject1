using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Scripts.Classes.Helper.Pilot
{
    class MoveToward : Action
    {
        public SharedVector3 Target;
        public SharedVector2 ControlStick;
        public SharedSpaceship TargetSpaceship;
        public SharedFloat TargetSpeed;
        public SharedFloat MaxSpeed = 3f;
        public SharedFloat AcceptableDistance = 1;
        public SharedBool SlowArrive = true;

        private float stoppingDistance = 0f;
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
            Vector2 stick = new Vector2();
            float targetAngle = Vector3.Angle((transform.forward).normalized, (targetPosition - transform.position).normalized);
            if (isLeft(transform.position, transform.position + transform.forward * 500, targetPosition))
            {
                targetAngle = -targetAngle;
            }

            Debug.Log(stoppingDistance);
            Vector3 temp = targetPosition - transform.position;
            
            stick = new Vector2(temp.x, temp.z).normalized;

            Debug.DrawLine(transform.position, targetPosition, Color.white, BehaviorManager.instance.UpdateIntervalSeconds);
            Debug.DrawLine(transform.position, transform.position + transform.forward, Color.white, BehaviorManager.instance.UpdateIntervalSeconds);

            ControlStick.Value = stick;
            float dot = Vector3.Dot(temp, transform.forward);
            TargetSpeed.Value = Mathf.Clamp(dot * MaxSpeed.Value, 0f, MaxSpeed.Value);

            if (SlowArrive.Value)
            {
                Vector3 proj = Math3d.ProjectPointOnLine(transform.position, transform.forward, targetPosition);
                int result = Math3d.PointOnWhichSideOfLineSegment(transform.position,
                    transform.position + transform.forward * stoppingDistance, proj);
                
                if (result == 0)
                {
                    TargetSpeed.Value = 0;
                }
                if (rigidbody.velocity.magnitude < 0.05f && Vector3.Magnitude(transform.position - Target.Value) < AcceptableDistance.Value)
                {
                    TargetSpeed.Value = 0;
                    Shipscript.GetPilot.TargetFaceDirection = transform.forward;
                    return TaskStatus.Success;
                }
                Debug.DrawLine(transform.position + transform.forward * stoppingDistance, transform.position + transform.forward * stoppingDistance + transform.up * 10f);
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
