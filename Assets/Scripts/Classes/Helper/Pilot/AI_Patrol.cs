using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI_Missions;
using ShipInternals;

public class AI_Patrol : PilotInterface
{
	// Use this for initialization
	void Start ()
    {
        base.Start();
        ai_type = AI_Type.PATROL;
        mySensorArray = new SensorArray(gameObject);
        _missions.Add(new TravelTo(gameObject, new Vector3(10f, transform.position.y, -10f)));
        _missions.Add(new TravelTo(gameObject, new Vector3(10f, transform.position.y, -20f)));
        _missions.Add(new TravelTo(gameObject, new Vector3(10f, transform.position.y, -40f)));
        //_missions.Add(new TravelTo(gameObject, new Vector3(-10f, transform.position.y, 10f)));
        //_missions.Add(new TravelTo(gameObject, new Vector3(-10f, transform.position.y, -10f)));
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //base.FixedUpdate();
        if (_missions.Count >= 1 && _missions.Count > missionIndex)
        {
            //Debug.Log(missionIndex);
            //Debug.Log(_missions[missionIndex].State);
            _missions[missionIndex].AI_Update();
            control_stickDirection = _missions[missionIndex].Stick;
            targetSpeed = _missions[missionIndex].TargetSpeed;
            if (_missions[missionIndex].State == AI_States.DONE)
            {
                missionIndex++;
                if (missionIndex == _missions.Count)
                {
                    missionIndex = 0;
                    foreach (var mission in _missions)
                    {
                        if(mission.State == AI_States.DONE)
                            mission.Reset();
                    }
                }
                _missions[missionIndex].SetMissionIndex(missionIndex);
            }
        }
	}
}
