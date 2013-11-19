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

        public TravelTo(GameObject parent_in, Vector2 target1_in)
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
                        Vector3 temp = _target1 - _parent.transform.position;
                        //temp = temp.normalized + _parent.transform.forward;
                        stick = new Vector2(temp.x, temp.z);
                        Debug.DrawLine(_parent.transform.position, _parent.transform.position + _parent.transform.forward);
                        if (temp.magnitude < 10)
                        {
                            stick = new Vector2();
                            _AI_State = AI_States.ARRIVED;
                        }
                        stick.y = 0;
                        break;
                    }
                case AI_States.ARRIVED:
                    {
                        break;
                    }
            }
        }

        public Vector3 Stick
        {
            get { return stick.normalized; }
        }
    }
}
