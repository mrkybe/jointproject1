using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Static : MonoBehaviour
{
    static public bool inTime = true;
    protected int loadPriority = 0;
    protected int loadPriorityInital = 5;
    [SerializeField]
    protected float interactionRange = 1;
    public static List<Static> listOfStaticObjects;

    public void Start()
    {
        loadPriority = loadPriorityInital;
        listOfStaticObjects = new List<Static>();
        listOfStaticObjects.Add(this);
	}

    protected virtual void DelayedLoad()
    {
        
    }

	// Update is called once per frame
	protected void FixedUpdate ()
    {
        
	}
}
