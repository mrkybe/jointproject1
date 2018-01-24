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
        [SerializeField]
        public List<GameObject> Meshes = new List<GameObject>();

        [SerializeField]
        [Range(0, 11)]
        public int modelNumber = 0;

        private MeshFilter myMesh;
        private MeshRenderer myMeshRenderer;
        private void Awake()
        {
            myMesh = this.GetComponent<MeshFilter>();
            myMeshRenderer = this.GetComponent<MeshRenderer>();

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

            float x = (Random.value * 2 + 2); // width
            float y = (Random.value * 4 + 2); // length
            float z = (Random.value * 2 + 2); // height
            if (Random.value > 0.5f)
            {
                z = z * -1;
            }

            Vector3 scale = new Vector3(x,y,z);
            this.transform.localScale = scale;


        }
    }
}
