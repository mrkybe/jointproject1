using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
    private float createdTime;
    private float timeLength;
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
            timeLength -= Time.deltaTime;
        }
        if (timeLength < 0)
        {
            done = true;
            ticking = false;

            Debug.Log("TICKIn");
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
        timeLength = length;
        savedLength = length;
        Reset();
    }

    public bool isDone()
    {
        return done;
    }

    public bool isTicking()
    {
        return ticking;
    }

    public float timeLeft()
    {
        return timeLength;
    }

    public float getSetLength()
    {
        return savedLength;
    }

    public float getCreatedTime()
    {
        return createdTime;
    }
}

