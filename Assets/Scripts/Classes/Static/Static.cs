using System.Collections.Generic;
using Assets.Scripts.Classes.WorldSingleton;
using UnityEngine;

namespace Assets.Scripts.Classes.Static
{
    [SelectionBase]
    public class Static : MonoBehaviour
    {
        protected int loadPriority = 0;
        protected int loadPriorityInital = 5;
        protected float interactionRange = 1;
        public static List<Static> listOfStaticObjects = new List<Static>();
        Overseer BossScript;

        protected void Start()
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

        protected void OnDestroy()
        {
            listOfStaticObjects.Remove(this);
        }

        public void Pause()
        {

        }

        public void Unpause()
        {

        }
    }
}
