﻿using System;
using Assets.Scripts.Classes.Mobile;
using Assets.Scripts.Classes.WorldSingleton;
using UnityEngine;

namespace Assets.Scripts.Classes.Helper.Pilot {
    public class PlayerPilot : PilotInterface
    {
        private Spaceship myShip = null;
        // Use this for initialization
        new void Start ()
        {
            base.Start();
            myShip = GetComponent<Spaceship>();
            Faction = Overseer.Main.GetFaction("Player");
        }
	
        // Update is called once per frame
        new void Update ()
        {
            base.Update();
            throttle = Input.GetAxis("Vertical");


            //Vector3.RotateTowards(targetFaceDirection);
            //Quaternion rot = Quaternion.Euler(Input.GetAxis("Horizontal") * Vector3.up);
            Quaternion rot = Quaternion.Euler(Input.GetAxis("Horizontal") * Vector3.up * Time.deltaTime * 120f);
            targetFaceDirection = rot * targetFaceDirection;
        }

        public override void Die()
        {
            // Game Over!
            throw new NotImplementedException();
        }
    }
}
