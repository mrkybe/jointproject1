using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAsteroid : MonoBehaviour
{
    [SerializeField]
    private List<Mesh> AsteroidModels;

    private MeshRenderer mr;
    private MeshFilter mf;
    private MeshCollider mc;
    private Rigidbody rb;

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        mf = GetComponent<MeshFilter>();
        mc = GetComponent<MeshCollider>();
        rb = GetComponent<Rigidbody>();

        float rx = 1 + Random.value;
        float ry = 1 + Random.value;
        float rz = 1 + Random.value;
        float rs = 1 + Random.value;
        Vector3 scale = new Vector3(rx, ry, rz) * rs * rs;

        this.transform.rotation = Random.rotation;
        transform.localScale = scale;
        rb.mass = scale.sqrMagnitude;
    }

    // Use this for initialization
    void Start()
    {
        mf.mesh = AsteroidModels[Random.Range(0, AsteroidModels.Count)];
        mc.sharedMesh = mf.mesh;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
