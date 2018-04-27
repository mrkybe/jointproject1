using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Classes.Mobile;
using Assets.Scripts.Classes.WorldSingleton;
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

        private List<GameObject> Trails = new List<GameObject>();


        [SerializeField]
        public List<Vector3> TrailPositionsM0 = new List<Vector3>();
        [SerializeField]
        public List<Vector3> TrailPositionsM1 = new List<Vector3>();
        [SerializeField]
        public List<Vector3> TrailPositionsM2 = new List<Vector3>();
        [SerializeField]
        public List<Vector3> TrailPositionsM3 = new List<Vector3>();
        [SerializeField]
        public List<Vector3> TrailPositionsM4 = new List<Vector3>();
        [SerializeField]
        public List<Vector3> TrailPositionsM5 = new List<Vector3>();
        [SerializeField]
        public List<Vector3> TrailPositionsM6 = new List<Vector3>();
        [SerializeField]
        public List<Vector3> TrailPositionsM7 = new List<Vector3>();
        [SerializeField]
        public List<Vector3> TrailPositionsM8 = new List<Vector3>();
        [SerializeField]
        public List<Vector3> TrailPositionsM9 = new List<Vector3>();
        [SerializeField]
        public List<Vector3> TrailPositionsM10 = new List<Vector3>();
        [SerializeField]
        public List<Vector3> TrailPositionsM11 = new List<Vector3>();

        private List<List<Vector3>> TrailPositions = new List<List<Vector3>>();

        private List<TrailRenderer> TrailRenderers = new List<TrailRenderer>();

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
        private GameObject myIndicator;

        private GameObject myTrailsSource;

        
        private void Awake()
        {
            myMesh = this.GetComponent<MeshFilter>();
            myMeshRenderer = this.GetComponent<MeshRenderer>();
            myParticleSystem = this.GetComponent<ParticleSystem>();
            mySpaceshipScript = this.transform.parent.GetComponent<Spaceship>();
            mySensorCollider = this.GetComponent<SphereCollider>();
            mySpaceshipParentRigidbody = mySpaceshipScript.GetComponent<Rigidbody>();
            for (int i = 0; i < this.transform.parent.transform.childCount; i++)
            {
                if (this.transform.parent.transform.GetChild(i).gameObject.name == "Indicator")
                {
                    myIndicator = this.transform.parent.transform.GetChild(i).gameObject;
                }
            }

            for (int i = 0; i < this.transform.childCount; i++)
            {
                if (this.transform.GetChild(i).gameObject.name == "TrailSource")
                {
                    myTrailsSource = this.transform.GetChild(i).gameObject;
                    TrailRenderers.Add(myTrailsSource.GetComponent<TrailRenderer>());
                    Trails.Add(myTrailsSource);
                }
            }
            initialRotation = transform.localRotation;
            myParticleSystem.Stop();

            TrailPositions = new List<List<Vector3>>
            {
                TrailPositionsM0,
                TrailPositionsM1,
                TrailPositionsM2,
                TrailPositionsM3,
                TrailPositionsM4,
                TrailPositionsM5,
                TrailPositionsM6,
                TrailPositionsM7,
                TrailPositionsM8,
                TrailPositionsM9,
                TrailPositionsM10,
                TrailPositionsM11
            };

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
            //InvokeRepeating("UpdateTrailRenderers", 1f, (0.1f));
        }

        private void ConfigureTrails(List<Vector3> Positions)
        {
            int count = 0;
            foreach (Vector3 pos in Positions)
            {
                if (count < Trails.Count)
                {
                    Trails[count].transform.localPosition = pos;
                }
                else
                {
                    GameObject newTrail = Instantiate(myTrailsSource, transform.position, Quaternion.identity);
                    newTrail.transform.parent = this.transform;
                    newTrail.transform.localPosition = pos;
                    TrailRenderers.Add(newTrail.GetComponent<TrailRenderer>());
                    Trails.Add(newTrail);
                }
                count++;
            }
            while (Trails.Count > Positions.Count)
            {
                GameObject last = Trails[Trails.Count - 1];
                TrailRenderers.Remove(TrailRenderers.First(x => x.gameObject == last));
                Trails.RemoveAt(Trails.Count - 1);
                Destroy(last);
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

            if (TrailPositions[modelNumber].Count != 0)
            {
                ConfigureTrails(TrailPositions[modelNumber]);
            }
            
        }

        public void SetColor(Color color)
        {
            myMeshRenderer.material.color = color;
            if (myIndicator != null)
            {
                myIndicator.GetComponent<MeshRenderer>().material.color = color;
            }
        }

        public void SetSensorRange(float radius)
        {
            Vector3 localScale = this.transform.localScale;
            float scaleMultiplier = Mathf.Max(localScale.x, localScale.y, localScale.z);
            mySensorCollider.radius = radius / scaleMultiplier;
        }

        public void UpdateTrailRenderers()
        {
            if (!Overseer.Main.IsOvermapPaused())
            {
                foreach (TrailRenderer t in TrailRenderers)
                {
                    t.time = Mathf.Clamp(Mathf.Log(mySpaceshipParentRigidbody.velocity.magnitude, 1.5f), 0, 4);
                }
            }
            else
            {
                foreach (TrailRenderer t in TrailRenderers)
                {
                    //t.time = float.MaxValue;
                }
            }
        }

        public void Update()
        {
            if (State == States.DEAD)
            {
                transform.Rotate(randomRotationAxis, rotationSpeed * Time.deltaTime);
            }
            else
            {
                UpdateTrailRenderers();
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
            Overseer.Main.DoExplosion(this.transform.position, 11);
            Quaternion randomSpin = Random.rotationUniform;
            //transform.rotation = Random.rotation;

            randomSpin.ToAngleAxis(out rotationSpeed, out randomRotationAxis);
            rotationSpeed = (0.5f + Random.value) * 20f;
        }
    }
}
