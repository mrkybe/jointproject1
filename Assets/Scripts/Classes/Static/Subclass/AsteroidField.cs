using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShipInternals;

public class AsteroidField : Static
{
    int rawMaterial;
    private CargoHold myStorage;
    private int maxStorage = 200;
    List<Vector3> vertices = new List<Vector3>();
    // Use this for initialization
    void Start ()
    {
        base.Start();
        myStorage = new CargoHold(maxStorage);
        //GenerateMesh();
        var m = GenerateAsteroid(1);
        GetComponent<MeshFilter>().mesh = m;
    }

    new protected void DelayedLoad()
    {
        myStorage.addHoldType("Gold");
        myStorage.addToHold("Gold", 200);
        //Debug.Log("PRINTING HOLD FOR ASTEROID FIELD");
        
        //Debug.Log("-NOTE: STAGE " + loadPriorityInital + " LOADING COMPLETE");
    }

    public CargoHold GetCargoHold
    { 
        get { return myStorage; } 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            myStorage.printHold();
        }
    }

    Mesh GenerateAsteroid(float size)
    {
        System.Random r = new System.Random(this.GetInstanceID());
        Mesh mesh = new Mesh();
        vertices.Add(new Vector3(0, 0, 0));
        float vertsTotal = 3+(size/2);
        for (int i = -((int)vertsTotal); i < vertsTotal; i++)
        {
            var vec = new Vector3(Mathf.Sin(Mathf.PI * ((float)i / vertsTotal)),
                                  0,
                                  Mathf.Cos(Mathf.PI * ((float)i / vertsTotal)));
            float offset = (float)(r.NextDouble() * size + (size));
            vec *= offset;
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

    void GenerateMesh()
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
    new void FixedUpdate ()
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
