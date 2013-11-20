using UnityEngine;
using System.Collections;

public class Static : MonoBehaviour
{
    static public bool inTime = true;
    protected int loadPriority = 0;
    protected int loadPriorityInital = 5;
    [SerializeField]
    protected float interactionRange = 1;

    public void Start()
    {
        loadPriority = loadPriorityInital;
	}

    protected virtual void DelayedLoad()
    {
        
    }
	
	// Update is called once per frame
	protected void FixedUpdate ()
    {
        
	}
}
