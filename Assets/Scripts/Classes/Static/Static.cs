using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Static : MonoBehaviour
{
    static public bool inTime = true;
    protected int loadPriority = 0;
    protected int loadPriorityInital = 5;
    protected float interactionRange = 1;
    public static List<Static> listOfStaticObjects = new List<Static>();
    Overseer BossScript;

    public void Start()
    {
        BossScript = GameObject.Find("Overseer").GetComponent<Overseer>();
        if(BossScript != null)
        {

        }
        loadPriority = loadPriorityInital;
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
