using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Scripts.Classes.Helper.Pilot
{
    class MoveToward : Action
    {
        public SharedTransform Target;
        public SharedVector2 ControlStick;
        public SharedFloat TargetSpeed;

        public override TaskStatus OnUpdate()
        {
            Vector3 targetPosition = Target.Value.position;
            Vector2 stick = new Vector2();
            float targetAngle = Vector3.Angle((transform.forward).normalized, (targetPosition - transform.position).normalized);
            if (isLeft(transform.position, transform.position + transform.forward * 500, targetPosition))
            {
                targetAngle = -targetAngle;
            }


            //Debug.Log(targetSpeed);
            Vector3 temp = targetPosition - transform.position;

            // TODO:  Make this more accurate by using _parent.transform.forward - target position.
            /*if (temp.magnitude < mySensorArray.StoppingDistance)
            {
                bool slowDown = true;
            }*/
            //temp = temp.normalized + _parent.transform.forward;
            stick = new Vector2(temp.x, temp.z);

            Debug.DrawLine(transform.position, targetPosition, Color.white, BehaviorManager.instance.UpdateIntervalSeconds);
            Debug.DrawLine(transform.position, transform.position + transform.forward, Color.white, BehaviorManager.instance.UpdateIntervalSeconds);
            stick.y = 0;
            stick.x = targetAngle / 10;

            stick.x = Mathf.Clamp(stick.x, -1f, 1f);
            stick.y = Mathf.Clamp(stick.y, -1f, 1f);

            // Outputs
            ControlStick.Value = stick;
            TargetSpeed.Value = Mathf.Clamp((Mathf.Clamp01(1 - (Mathf.Abs(targetAngle) / 60)) * 3), 0f, 10f);

            if (Vector3.SqrMagnitude(transform.position - Target.Value.position) < 0.1f)
            {
                return TaskStatus.Success;
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
