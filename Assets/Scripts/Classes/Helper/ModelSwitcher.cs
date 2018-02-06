using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Classes.Helper
{
    class ModelSwitcher : MonoBehaviour
    {
        public enum States { ALIVE, DEAD }

        [SerializeField]
        public List<GameObject> Meshes = new List<GameObject>();

        [SerializeField]
        [Range(0, 11)]
        public int modelNumber = 0;

        private MeshFilter myMesh;
        private MeshRenderer myMeshRenderer;
        private ParticleSystem myParticleSystem;
        private Spaceship mySpaceshipScript;
        private States State = States.ALIVE;

        
        private void Awake()
        {
            myMesh = this.GetComponent<MeshFilter>();
            myMeshRenderer = this.GetComponent<MeshRenderer>();
            myParticleSystem = this.GetComponent<ParticleSystem>();
            mySpaceshipScript = this.transform.parent.GetComponent<Spaceship>();

            myParticleSystem.Stop();

            if (modelNumber < Meshes.Count)
            {
                myMesh.mesh = Meshes[modelNumber].GetComponent<MeshFilter>().sharedMesh;
                myMeshRenderer.material = Meshes[modelNumber].GetComponent<MeshRenderer>().sharedMaterial;
            }
        }

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
                z = z * -1;
            }

            Vector3 scale = new Vector3(x,y,z);
            this.transform.localScale = scale;
        }

        public void Update()
        {
            if (State == States.DEAD)
            {
                transform.Rotate(randomRotationAxis, rotationSpeed * Time.deltaTime);
            }
        }

        public void OnTriggerEnter(Collider collider)
        {
            mySpaceshipScript.SensorEnter(collider);

        }

        public void OnTriggerExit(Collider collider)
        {
            mySpaceshipScript.SensorExit(collider);
        }

        private Vector3 randomRotationAxis;
        private float rotationSpeed;

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
