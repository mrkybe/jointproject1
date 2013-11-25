using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI_Missions;
using EntityParts;

public class AI_Gather : PilotInterface
{
    private List<TravelTo> _missions;
    private SensorArray mySensorArray;
	// Use this for initialization
	void Start ()
    {
        base.Start();
        mySensorArray = new SensorArray(gameObject);
        _missions = new List<TravelTo>();
        _missions.Add(new TravelTo(gameObject, new Vector3(10, transform.position.y, 10)));
        _missions.Add(new TravelTo(gameObject, new Vector3(40, transform.position.y, 10)));
        _missions.Add(new TravelTo(gameObject, new Vector3(40, transform.position.y, 40)));
        _missions.Add(new TravelTo(gameObject, new Vector3(10, transform.position.y, 40)));
        _missions.Add(new TravelTo(gameObject, new Vector3(10, transform.position.y, 10)));
	}
	
	// Update is called once per frame
	void Update ()
    {
        base.Update();
        if (_missions.Count >= 1)
        {
            _missions[0].AI_Update();
            control_stickDirection = _missions[0].Stick;
            targetSpeed = _missions[0].TargetSpeed;
            if (_missions[0].Done)
            {
                _missions.RemoveAt(0);
            }
        }
	}

    public SensorArray SensorArray
    {
        get { return mySensorArray; }
    }

    public List<TravelTo> Missions
    {
        get { return _missions; }
    }
}
