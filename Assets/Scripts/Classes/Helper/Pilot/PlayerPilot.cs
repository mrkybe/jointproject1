using System;
using Assets.Scripts.Classes.Mobile;
using Assets.Scripts.Classes.WorldSingleton;
using UnityEngine;

namespace Assets.Scripts.Classes.Helper.Pilot {
    public class PlayerPilot : PilotInterface
    {
        public enum ControlMode { CONTROLLER, KEYBOARD};
        private Spaceship myShip = null;

        public ControlMode controlMode = ControlMode.CONTROLLER;
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
            if (controlMode == ControlMode.KEYBOARD)
            {
                throttle = Input.GetAxis("Vertical");
                Quaternion rot = Quaternion.Euler(Input.GetAxis("Horizontal") * Vector3.up * Time.deltaTime * 120f);
                targetFaceDirection = rot * targetFaceDirection;
            }
            else if (controlMode == ControlMode.CONTROLLER)
            {
                Vector3 fd = new Vector3(Input.GetAxis("Horizontal"), 0 , Input.GetAxis("Vertical"));
                throttle = Input.GetAxis("RightTrigger") - Input.GetAxis("LeftTrigger");
                if (fd.magnitude > 0.01f)
                {
                    fd.Normalize();
                    targetFaceDirection = fd;
                }
            }
        }

        public override void Die(Spaceship killer = null)
        {
            // Game Over!
            Invoke("LoseGame", 1);
            //throw new NotImplementedException();
        }

        private void LoseGame()
        {
            Overseer.Main.PauseOvermap();
        }

        public override void Pause()
        {
            //throw new NotImplementedException();
        }

        public override void Unpause()
        {
            //throw new NotImplementedException();
        }
    }
}
