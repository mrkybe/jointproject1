using UnityEngine;

namespace Assets.Scripts.Classes.Helper {
    public class Timer : MonoBehaviour
    {
        /* Written by Ruslan Kaybyshev, Middle 2013
     * Purpose: Easily add a timer component to any game object.
     * Example: // Declare a reference to the timer.
     *          private Timer alarmClock;
	 *          void Start ()
     *          {
     *             // Create and add it to the gameObject as a component
     *             // while saving the reference to it in the script.
     *             alarmClock = gameObject.AddComponent<Timer>();
     *             alarmClock.SetTimer(1);
	 *          }
     */
        private float createdTime;
        private float timeLeft;
        private float savedLength;
        private bool ticking;
        private bool done;
        private bool loop = false;

        void Start()
        {
            createdTime = Time.time;
            done = false;
            ticking = true;
        }

        void FixedUpdate()
        {
            if (ticking)
            {
                timeLeft -= Time.deltaTime;
            }
            if (timeLeft < 0)
            {
                done = true;
                ticking = false;

                if (loop)
                {
                    SetTimer(savedLength);
                }
            }
        }

        public void Reset()
        {
            done = false;
            ticking = true;
        }

        public void Pause(bool p)
        {
            ticking = !p;
        }

        public void Loop(bool p)
        {
            loop = p;
        }

        public void SetTimer(float length)
        {
            timeLeft = length;
            savedLength = length;
            Reset();
        }

        public bool Done
        {
            get { return done; }
        }

        public bool Ticking
        {
            get { return ticking; }
            set { ticking = value; }
        }

        public float TimeLeft
        {
            get { return timeLeft; }
        }

        public float StartTime
        {
            get { return savedLength; }
        }

        public float CreatedTime
        {
            get { return createdTime; }
        }
    }
}

