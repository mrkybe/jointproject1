using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI_Missions
{
    
    public class TravelTo
    {
        enum AI_States { MISSION_START, EN_ROUTE, ARRIVED };
        AI_States _AI_State;
        Vector3 _target1;
        GameObject _parent;
        
        Vector2 stick;

        public TravelTo(GameObject parent_in, Vector3 target1_in)
        {
            _parent = parent_in;
            _target1 = target1_in;
            _AI_State = AI_States.MISSION_START;
        }

        public void AI_Update()
        {
            switch (_AI_State)
            {
                case AI_States.MISSION_START:
                    {
                        _AI_State = AI_States.EN_ROUTE;
                        break;
                    }
                case AI_States.EN_ROUTE:
                    {
                        float targetAngle = Vector3.Angle((_parent.transform.forward).normalized, (_target1-_parent.transform.position).normalized);
                        if (isLeft(_parent.transform.position, _parent.transform.position + _parent.transform.forward * 500, _target1))
                        {
                            targetAngle = -targetAngle;
                        }
                        Debug.Log(targetAngle);
                        Vector3 temp = _target1 - _parent.transform.position;
                        //temp = temp.normalized + _parent.transform.forward;
                        stick = new Vector2(temp.x, temp.z);
                        Debug.DrawLine(_parent.transform.position, _target1);
                        Debug.DrawLine(_parent.transform.position, _parent.transform.position + _parent.transform.forward);
                        stick.y = 0;
                        stick.x = targetAngle / 10 ;
                        break;
                    }
                case AI_States.ARRIVED:
                    {
                        break;
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
    }
}
