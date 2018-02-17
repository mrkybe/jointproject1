using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Classes.Mobile;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Classes.Helper
{
    /// <summary>
    /// Handles the in game apperence of the Spaceships.
    /// </summary>
    class ModelSwitcher : MonoBehaviour
    {
        /// <summary>
        /// The state of the ship.
        /// </summary>
        public enum States { ALIVE, DEAD }

        /// <summary>
        /// List of 3d Models for the Spaceships
        /// </summary>
        [SerializeField]
        public List<GameObject> Meshes = new List<GameObject>();

        /// <summary>
        /// Which model to use.
        /// </summary>
        [SerializeField]
        [Range(0, 11)]
        private int modelNumber = 0;

        private MeshFilter myMesh;
        private MeshRenderer myMeshRenderer;
        private ParticleSystem myParticleSystem;
        private Spaceship mySpaceshipScript;
        private Rigidbody mySpaceshipParentRigidbody;
        private SphereCollider mySensorCollider;
        private States State = States.ALIVE;
        private Quaternion initialRotation;

        
        private void Awake()
        {
            myMesh = this.GetComponent<MeshFilter>();
            myMeshRenderer = this.GetComponent<MeshRenderer>();
            myParticleSystem = this.GetComponent<ParticleSystem>();
            mySpaceshipScript = this.transform.parent.GetComponent<Spaceship>();
            mySensorCollider = this.GetComponent<SphereCollider>();
            mySpaceshipParentRigidbody = mySpaceshipScript.GetComponent<Rigidbody>();
            initialRotation = transform.localRotation;

            myParticleSystem.Stop();

            /*if (modelNumber < Meshes.Count)
            {
                myMesh.mesh = Meshes[modelNumber].GetComponent<MeshFilter>().sharedMesh;
                myMeshRenderer.material = Meshes[modelNumber].GetComponent<MeshRenderer>().sharedMaterial;
            }*/
        }

        public void Start()
        {
            if (mySpaceshipScript.Pilot != null && mySpaceshipScript.Pilot.Faction != null)
            {
                SetColor(mySpaceshipScript.Pilot.Faction.ColorPrimary);
            }
        }

        /// <summary>
        /// Changes the model of the ship.
        /// </summary>
        /// <param name="num">Which number to use.</param>
        /// <param name="randomize_scale"></param>
        public void SetModel(int num, bool randomize_scale = true)
        {
            if (num < Meshes.Count)
            {
                myMesh.mesh = Meshes[num].GetComponent<MeshFilter>().sharedMesh;
                myMeshRenderer.material = Meshes[num].GetComponent<MeshRenderer>().sharedMaterial;
            }

            float x = (Random.value * 2 + 4); // width
            float y = (Random.value * 2 + 4); // length
            float z = (Random.value * 2 + 4); // height
            if (Random.value > 0.5f)
            {
                //z = z * -1;
            }

            Vector3 scale = new Vector3(x,y,z);
            this.transform.localScale = scale;
            this.modelNumber = num;

            if (mySpaceshipScript.Pilot != null && mySpaceshipScript.Pilot.Faction != null)
            {
                SetColor(mySpaceshipScript.Pilot.Faction.ColorPrimary);
            }
        }

        public void SetColor(Color color)
        {
            myMeshRenderer.material.color = color;
        }

        public void SetSensorRange(float radius)
        {
            Vector3 localScale = this.transform.localScale;
            float scaleMultiplier = Mathf.Max(localScale.x, localScale.y, localScale.z);
            mySensorCollider.radius = radius / scaleMultiplier;
        }

        public void Update()
        {
            if (State == States.DEAD)
            {
                transform.Rotate(randomRotationAxis, rotationSpeed * Time.deltaTime);
            }
            else
            {
                //Vector3 angVel = mySpaceshipParentRigidbody.angularVelocity;
                //transform.Rotate(Vector3.up, angVel.y * Time.deltaTime * -75f);
                //transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation, Time.deltaTime);
            }
        }

        public void OnTriggerEnter(Collider collider)
        {
            // Don't detect ourselves... obviously.
            if(collider.GetComponent<Spaceship>() != mySpaceshipScript)
                mySpaceshipScript.SensorEnter(collider);
        }

        public void OnTriggerExit(Collider collider)
        {
            mySpaceshipScript.SensorExit(collider);
        }

        private Vector3 randomRotationAxis;
        private float rotationSpeed;

        /// <summary>
        /// Make the ship appear dead.
        /// </summary>
        public void BecomeGraveyard()
        {
            State = States.DEAD;
            myParticleSystem.Play();
            Quaternion randomSpin = Random.rotationUniform;
            //transform.rotation = Random.rotation;

            randomSpin.ToAngleAxis(out rotationSpeed, out randomRotationAxis);
            rotationSpeed = (0.5f + Random.value) * 20f;
        }
    }
}
