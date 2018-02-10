using System.Collections.Generic;
using Assets.Scripts.Classes.Helper.ShipInternals;
using UnityEngine;

namespace Assets.Scripts.Classes.Static {
    /// <summary>
    /// A minable asteroid field.
    /// </summary>
    public class AsteroidField : Static
    {
        int rawMaterial;
        private float rotationSpeed = 1f;
        private int maxStorage = 200;
        List<Vector3> vertices = new List<Vector3>();
        public static List<AsteroidField> listOfAsteroidFields = new List<AsteroidField>();
        // Use this for initialization
        protected new void Start ()
        {
            base.Start();
            CargoHold = CargoHold.GenerateAsteroidFieldCargoHold();
            //GenerateMesh();
            System.Random r = new System.Random(this.GetInstanceID());
            float size = ((float) r.NextDouble() + 0.5f) * 2.5f;
            rotationSpeed = (1 / size)*10f;
            GetComponent<SphereCollider>().radius = size/1.125f;
            /*transform.position += Vector3.up;
        transform.position -= (Vector3.up * size);*/
            var m = GenerateAsteroid((float)size, Vector3.zero);
            GetComponent<MeshFilter>().mesh = m;
            listOfAsteroidFields.Add(this);
        }

        protected new void OnDestroy()
        {
            listOfAsteroidFields.Remove(this);
        }

        protected new void DelayedLoad()
        {
        
        }
        /// <summary>
        /// Returns the Cargohold of the AsteroidField.
        /// </summary>
        public CargoHold CargoHold
        {
            get; private set;
        }

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                CargoHold.PrintHold();
            }

            transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed * -1f);
        }

        private Mesh GenerateAsteroid(float size, Vector3 displacement)
        {
            System.Random r = new System.Random(this.GetInstanceID());
            Mesh mesh = new Mesh();
            vertices.Add(displacement);
            float vertsTotal = 3+(size/2);
            for (int i = -((int)vertsTotal); i < vertsTotal; i++)
            {
                var vec = new Vector3(Mathf.Sin(Mathf.PI * ((float)i / vertsTotal)),
                                      0,
                                      Mathf.Cos(Mathf.PI * ((float)i / vertsTotal)));
                float offset = (float)(r.NextDouble() * size + (size));
                vec *= offset;
                vec += displacement;
                vertices.Add(vec);
            }
            mesh.vertices = vertices.ToArray();
            mesh.name = "Procedural Asteroid";
            List<int> tris = new List<int>();
            for (int i = 1; i < vertices.Count - 1; i++)
            {
                tris.Add(0);
                tris.Add(i);
                tris.Add(i + 1);
            }
            tris.Add(0);
            tris.Add(vertices.Count - 1);
            tris.Add(1);
            mesh.triangles = tris.ToArray();
            return mesh;
        }

        private void GenerateMesh()
        {
            System.Random r = new System.Random(this.GetInstanceID());
            Mesh mesh = new Mesh();
            vertices.Add(new Vector3(0,0,0));
            float vertsTotal = 3;
            for (int i = -((int)vertsTotal); i < vertsTotal; i++)
            {
                var vec = new Vector3(Mathf.Sin(Mathf.PI * ((float)i / vertsTotal)),
                                      0,
                                      Mathf.Cos(Mathf.PI * ((float)i / vertsTotal)));
                float offset = (float)(r.NextDouble() + 1f);
                vec *= offset;
                vertices.Add(vec);
            }
            mesh.vertices = vertices.ToArray();
            mesh.name = "Procedural Asteroid";
            List<int> tris = new List<int>();
            for (int i = 1; i < vertices.Count-1; i++)
            {
                tris.Add(0);
                tris.Add(i);
                tris.Add(i+1);
            }
            tris.Add(0);
            tris.Add(vertices.Count-1);
            tris.Add(1);
            mesh.triangles = tris.ToArray();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        private void OnDrawGizmos()
        {
            /*Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Count; i++)
        {
            Gizmos.DrawSphere(vertices[i] + transform.position, 0.1f);
        }*/
        }

        // Update is called once per frame
        private new void FixedUpdate ()
        {
        
            if (inTime)
            {
                base.FixedUpdate();
                ///////////////////
                if (loadPriority == 0)
                {
                    DelayedLoad();
                    loadPriority = -1;
                }
                else if (loadPriority > 0)
                {
                    loadPriority--;
                }
            }
        }
    }
}
