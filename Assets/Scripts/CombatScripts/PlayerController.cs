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
  //  private LaserFire lf;
    // Use this for initialization
    void Start()
    {
        fire = GetComponent<Fire>();
        //lf = GetComponent<LaserFire>();
    }
    private void Update()
    {
		//&& fire.enabled == true)
		if(Input.GetButtonDown("LB"))
        {
            //fire.enabled = false;
            //lf.enabled = true;
			fire.ammo = fire.laser;
        }
		if (Input.GetButtonDown("LB"))
        {
            //lf.enabled = false;
            //fire.enabled = true;
			fire.ammo = fire.bullet;
        }
    }
}
