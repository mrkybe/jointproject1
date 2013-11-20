using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using EntityParts;

namespace AI_Missions
{
    
    public class TravelTo
    {
        enum AI_States { MISSION_START, EN_ROUTE, ARRIVED, ARRIVING };
        AI_States _AI_State;
        Vector3 _target1;
        GameObject _parent;
        float targetSpeed;
        Vector2 stick;
        bool slowDown;
        SensorArray mySensorArray;
        public TravelTo(GameObject parent_in, Vector3 target1_in)
        {
            _parent = parent_in;
            mySensorArray = _parent.GetComponent<AI_Gather>().SensorArray;
            _target1 = target1_in;
            _AI_State = AI_States.MISSION_START;
            targetSpeed = 0;
            slowDown = false;
        }

        public void AI_Update()
        {
            switch (_AI_State)
            {
                case AI_States.MISSION_START:
                    {
                        _AI_State = AI_States.EN_ROUTE;
                        targetSpeed = 0;
                        break;
                    }
                case AI_States.EN_ROUTE:
                    {
                        float targetAngle = Vector3.Angle((_parent.transform.forward).normalized, (_target1-_parent.transform.position).normalized);
                        if (isLeft(_parent.transform.position, _parent.transform.position + _parent.transform.forward * 500, _target1))
                        {
                            targetAngle = -targetAngle;
                            if (targetAngle > 60 || targetAngle < -60)
                            {
                                targetSpeed = 0.6f;
                            }
                            else
                            {
                                targetSpeed = 3;
                            }
                        }
                        if (slowDown)
                        {
                            targetSpeed = 0;
                            _AI_State = AI_States.ARRIVING;
                        }
                        //Debug.Log(targetAngle);
                        Vector3 temp = _target1 - _parent.transform.position;
                        if (temp.magnitude < mySensorArray.StoppingDistance)
                        {
                            slowDown = true;
                        }
                        //temp = temp.normalized + _parent.transform.forward;
                        stick = new Vector2(temp.x, temp.z);
                        Debug.DrawLine(_parent.transform.position, _target1);
                        Debug.DrawLine(_parent.transform.position, _parent.transform.position + _parent.transform.forward);
                        stick.y = 0;
                        stick.x = targetAngle / 10 ;
                        break;
                    }
                case AI_States.ARRIVING:
                    {
                        Debug.Log("ARRIVING");
                        _AI_State = AI_States.ARRIVED;
                        stick.y = 0;
                        stick.x = 0;
                        break;
                    }
                case AI_States.ARRIVED:
                    {
                        break;
                        targetSpeed = 0;
                    }
            }

            stick.x = Mathf.Clamp(stick.x, -1f, 1f);
            stick.y = Mathf.Clamp(stick.y, -1f, 1f);
        }

        public bool isLeft(Vector3 pos1, Vector3 pos2, Vector3 checkPoint)
        {
            return ((pos2.x - pos1.x) * (checkPoint.z - pos1.z) - (pos2.z - pos1.z) * (checkPoint.x - pos1.x)) > 0;
        }

        public Vector3 Stick
        {
            get { return stick; }
        }

        public float TargetSpeed
        {
            get { return targetSpeed; }
        }
    }
}
