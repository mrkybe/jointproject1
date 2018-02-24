using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes;
using Assets.Scripts.Classes.Mobile;
using UnityEngine;


///<summary>
/// The PlayerController is repsonsible for controlling which objects are actuve during
/// combat. This class also enables and disables associated scripts with the Overworld
/// player.
///</summary>
public class PlayerController : MonoBehaviour
{
    private Fire fire;
	private LaserFire lf;
	private Rocket rk;
    // Use this for initialization
    void Start()
    {
        fire = GetComponent<Fire>();
        lf = GetComponent<LaserFire>();
		rk = GetComponent<Rocket> ();
    }
    private void Update()
    {
		//&& fire.enabled == true)
		if (Input.GetButtonDown ("LB") && fire.enabled == true) {
			Debug.Log ("Switching");
			fire.enabled = false;
			lf.enabled = true;
			rk.enabled = false;
			//fire.speed = 15f;
			//fire.ammo = fire.laser;

		} else if (Input.GetButtonDown ("LB") && lf.enabled == true) {
			Debug.Log ("Switching");
			fire.enabled = false;
			lf.enabled = false;
			rk.enabled = true;
			//fire.speed = 3f;
			//fire.ammo = fire.bullet;
		}
		else if (Input.GetButtonDown ("LB") && rk.enabled == true) 
		{
			Debug.Log ("Switching");
			fire.enabled = true;
			lf.enabled = false;
			rk.enabled = false;
		}
    }
}
