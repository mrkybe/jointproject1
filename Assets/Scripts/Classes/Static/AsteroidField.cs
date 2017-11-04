using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShipInternals;

public class AsteroidField : Static
{
    int rawMaterial;
    private CargoHold myStorage;
    private float rotationSpeed = 1f;
    private int maxStorage = 200;
    List<Vector3> vertices = new List<Vector3>();
    public static List<AsteroidField> listOfAsteroidFields = new List<AsteroidField>();
    // Use this for initialization
    void Start ()
    {
        base.Start();
        myStorage = new CargoHold(maxStorage);
        //GenerateMesh();
        System.Random r = new System.Random(this.GetInstanceID());
        float size = ((float) r.NextDouble() + 0.5f) * 2.5f;
        rotationSpeed = (1 / size)*10f;
        /*transform.position += Vector3.up;
        transform.position -= (Vector3.up * size);*/
        var m = GenerateAsteroid((float)size, Vector3.zero);
        GetComponent<MeshFilter>().mesh = m;
        listOfAsteroidFields.Add(this);
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

        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed * -1f);
    }

    Mesh GenerateAsteroidField()
    {
        System.Random r = new System.Random(this.GetInstanceID());
        Mesh mesh = new Mesh();
        List<Mesh> subAsteroids = new List<Mesh>();
        for (int i = 0; i < 2; i++)
        {
            float size = 1f;///((float)r.NextDouble() + 0.2f) * 1f;
            float offset_x = ((float)r.NextDouble() - 0.5f) * 10f;
            float offset_y = ((float)r.NextDouble() - 0.5f) * 10f;
            float offset_z = ((float)r.NextDouble() - 0.5f) * 10f;
            Vector3 offset = new Vector3(offset_x,offset_y,offset_z);
            Mesh asteroid = GenerateAsteroid(size, offset);
            subAsteroids.Add(asteroid);
        }
        CombineInstance[] combine = new CombineInstance[subAsteroids.Count];
        for (int i = 0; i < subAsteroids.Count; i++)
        {
            combine[i].mesh = subAsteroids[i];
            combine[i].transform = Matrix4x4.identity;
        }
        mesh.CombineMeshes(combine);
        mesh.name = subAsteroids[0].name;
        return mesh;
    }

    Mesh GenerateAsteroid(float size, Vector3 displacement)
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
