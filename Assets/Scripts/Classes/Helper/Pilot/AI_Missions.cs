using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ShipInternals;

namespace AI_Missions
{
    public enum AI_Type { GATHER, PATROL, PLAYER }

    public enum AI_States { MISSION_START, EN_ROUTE, ARRIVED, ARRIVING, DONE, MINING, WAITING};

    public class MissionGeneric
    {
        protected PilotInterface _parentAI;
        protected int myMissionIndex;
        protected List<MissionGeneric> MissionList;
        protected AI_States _AI_State;
        protected GameObject _parent;
        protected Spaceship shipScript;
        protected float targetSpeed;
        protected Vector2 stick;
        public MissionGeneric(GameObject parent_in)
        {
            _parent = parent_in;
            shipScript = _parent.transform.GetComponent<Spaceship>();
            _parentAI = _parent.transform.GetComponent<PilotInterface>();
            
            MissionList = _parentAI.GetMissionList();
            _AI_State = AI_States.MISSION_START;
            
            targetSpeed = 0;
            stick = Vector2.zero;
        }

        virtual public void AI_Update()
        {
            // Gets over ridden, never called
            Debug.Log("THIS SHOULD NEVER EVER HAPPEN.");
        }

        public void SetMissionIndex(int index)
        {
            myMissionIndex = index;
        }

        public AI_States State
        {
            get { return _AI_State; }
        }

        public float TargetSpeed
        {
            get { return targetSpeed; }
        }

        public Vector3 Stick
        {
            get { return stick; }
        }

        public virtual void Reset()
        {
            _AI_State = AI_States.MISSION_START;
        }
    }

    /*public class Wait : MissionGeneric
    {
        float timeToWaitFor = 0f;
        float timeStartedWaitingAt;
        public Wait(GameObject parent_in, float waitingLength) : base(parent_in)
        {
            timeToWaitFor = waitingLength;
        }

        public override void AI_Update()
        {
            if(myMissionIndex == _parentAI.GetMissionIndex())
            {
                switch(_AI_State)
                {
                    case AI_States.MISSION_START:
                        timeStartedWaitingAt = Time.time;
                        _AI_State = AI_States.WAITING;
                        break;

                    case AI_States.WAITING:
                        if(Time.time + timeToWaitFor < Time.time)
                        {
                            _AI_State = AI_States.DONE;
                        }
                        break;

                    case AI_States.DONE:
                        break;
                }
            }
        }
    }*/
    
    public class Mine : MissionGeneric
    {
        String miningTarget;
        int mineAmount;
        float timeSet = 0;

        public Mine(GameObject parent_in, String targetType) : base(parent_in)
        {
            mineAmount = 1;
            miningTarget = targetType;
        }

        public override void AI_Update()
        {
            //Debug.Log(myMissionIndex);
            if(myMissionIndex == _parentAI.GetMissionIndex())
            {
                //Debug.Log("MISSION INDEX MATCH");
                switch (_AI_State)
                {
                    case AI_States.MISSION_START:
                        timeSet = Time.time;
                        _AI_State = AI_States.WAITING;
                        break;
                    case AI_States.WAITING:
                        if((timeSet + 4) < Time.time)
                        {
                            _AI_State = AI_States.ARRIVED;
                        }
                        break;
                    case AI_States.ARRIVED:
                        if (shipScript != null)
                        {
                            List<Static> targets = shipScript.getAvailableTargets();
                            List<AsteroidField> newTargets = new List<AsteroidField>();
                            AsteroidField finalTarget;
                            Debug.Log("Targets available: " + targets.Count);
                            for (int i = 0; i < targets.Count; i++)
                            {
                                Debug.Log("MINE.MISSION_START");
                                if (targets[i].GetType() == typeof(AsteroidField))
                                {
                                    finalTarget = targets[i] as AsteroidField;
                                    if (finalTarget.GetCargoHold.Contains(miningTarget) &&
                                       finalTarget.GetCargoHold.GetAmountInHold(miningTarget) > 0)
                                    {
                                        newTargets.Add((AsteroidField)targets[i]);
                                    }
                                }

                            }
                            if (newTargets.Count >= 1)
                            {
                                finalTarget = newTargets[0];
                                finalTarget.GetCargoHold.AddToHold(miningTarget, -mineAmount);
                                shipScript.GetCargoHold.AddToHold(miningTarget, mineAmount);
                                Debug.Log("Taking Cargo");
                                _AI_State = AI_States.DONE;
                            }
                            _AI_State = AI_States.DONE;
                        }
                        break;
                }
            }
        }
    }

    public class TravelTo : MissionGeneric
    {
        Vector3 _target1;
        bool slowDown;
        SensorArray mySensorArray;

        public TravelTo(GameObject parent_in, Vector3 target1_in) : base(parent_in)
        {
            mySensorArray = _parent.GetComponent<PilotInterface>().SensorArray;
            _target1 = target1_in;
            
            _parentAI = _parent.GetComponent<PilotInterface>();
            targetSpeed = 0;
            slowDown = false;
        }

        public override void AI_Update()
        {
            switch (_AI_State)
            {
                case AI_States.MISSION_START:
                    {
                        _AI_State = AI_States.EN_ROUTE;
                        //Debug.Log("KAOSDKASO");
                        //targetSpeed = 0;
                        break;
                    }
                case AI_States.EN_ROUTE:
                    {
                        float targetAngle = Vector3.Angle((_parent.transform.forward).normalized, (_target1-_parent.transform.position).normalized);
                        if (isLeft(_parent.transform.position, _parent.transform.position + _parent.transform.forward * 500, _target1))
                        {
                            targetAngle = -targetAngle;
                        }

                        targetSpeed = Mathf.Clamp((Mathf.Clamp01(1 - (Mathf.Abs(targetAngle) / 60)) * 3), 0f, 3f);

                        if (slowDown)
                        {
                            targetSpeed = 0;
                            _AI_State = AI_States.ARRIVING;
                        }
                        //Debug.Log(targetSpeed);
                        Vector3 temp = _target1 - _parent.transform.position;

                        // TODO:  Make this more accurate by using _parent.transform.forward - target position.
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
                        Debug.Log("TravelTo.ARRIVING");
                        _AI_State = AI_States.ARRIVED;
                        stick.y = 0;
                        stick.x = 0;
                        break;
                    }
                case AI_States.ARRIVED:
                    {
                        Debug.Log("TravelTo.ARRIVED");
                        if (MissionList.Count < myMissionIndex && MissionList[myMissionIndex + 1].GetType() == this.GetType() )
                        {
                            targetSpeed = 0;
                        }
                        _AI_State = AI_States.DONE;
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

        public float TargetSpeed
        {
            get { return targetSpeed; }
        }
        
        public Vector3 Stick
        {
            get { return stick; }
        }

        public new void Reset()
        {
            base.Reset();
            targetSpeed = 0;
            slowDown = false;
        }
    }
}
