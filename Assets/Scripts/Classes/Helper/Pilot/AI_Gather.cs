using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI_Missions;
using ShipInternals;

public class AI_Gather : PilotInterface
{
    private List<MissionGeneric> _missions;
    private SensorArray mySensorArray;
    private int missionIndex;
	// Use this for initialization
	void Start ()
    {
        base.Start();
        mySensorArray = new SensorArray(gameObject);
        _missions = new List<MissionGeneric>();
        _missions.Add(new TravelTo(gameObject, new Vector3(19.1f, transform.position.y, -2.4f)));
        _missions.Add(new Mine(gameObject, "Ice"));
        _missions.Add(new TravelTo(gameObject, new Vector3(1119.1f, transform.position.y, -2.4f)));
	}
	
	// Update is called once per frame
	void Update ()
    {
        base.Update();
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

    public int GetMissionCount()
    {
        return _missions.Count;
    }

    public int GetMissionIndex()
    {
        return missionIndex;
    }

    public MissionGeneric GetMission(int x)
    {
        return _missions[x];
    }

    public List<MissionGeneric> GetMissionList()
    {
        return _missions;
    }
}
