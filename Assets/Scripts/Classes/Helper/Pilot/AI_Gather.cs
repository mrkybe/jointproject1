using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI_Missions;

public class AI_Gather : PilotInterface
{
    private List<TravelTo> _missions;
	// Use this for initialization
	void Start ()
    {
        _missions = new List<TravelTo>();
        _missions.Add(new TravelTo(gameObject, new Vector3(10, transform.position.y, 10)));
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_missions.Count >= 1)
        {
            _missions[0].AI_Update();
            control_stickDirection = _missions[0].Stick;
        }
	}
}
