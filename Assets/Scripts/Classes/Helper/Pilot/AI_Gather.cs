using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI_Missions;
using ShipInternals;

public class AI_Gather : PilotInterface
{
	// Use this for initialization
	void Start ()
    {
        base.Start();
        ai_type = AI_Type.GATHER;
        mySensorArray = new SensorArray(gameObject);
        _missions.Add(new TravelTo(gameObject, new Vector3(19.1f, transform.position.y, -2.4f)));
        _missions.Add(new Mine(gameObject, "Gold"));
        _missions.Add(new TravelTo(gameObject, new Vector3(19.1f, transform.position.y, -2.4f)));
        _missions.Add(new TravelTo(gameObject, new Vector3(0f, transform.position.y, -2.4f)));
        _missions.Add(new TravelTo(gameObject, new Vector3(19.1f, transform.position.y, -2.4f)));
        _missions.Add(new TravelTo(gameObject, new Vector3(0f, transform.position.y, -2.4f)));
        _missions.Add(new TravelTo(gameObject, new Vector3(19.1f, transform.position.y, -2.4f)));
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
            if (_missions[missionIndex].State == AI_Missions.AI_States.DONE)
            {
                missionIndex++;
                _missions[missionIndex].SetMissionIndex(missionIndex);
            }
        }
	}

    public SensorArray SensorArray
    {
        get { return mySensorArray; }
    }
}
