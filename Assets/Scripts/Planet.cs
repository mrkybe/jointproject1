using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour
{
    int population;

    [SerializeField]
    private GameObject workerShip;
    [SerializeField]
    private Timer timeToSpawn;
	// Use this for initialization
	void Start ()
    {
        timeToSpawn = gameObject.AddComponent<Timer>();
        timeToSpawn.SetTimer(1);
        //timeToSpawn.
        timeToSpawn.Loop(true);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (timeToSpawn.isDone())
        {
            Debug.Log("TICK TOCK");
        }
	}
}
