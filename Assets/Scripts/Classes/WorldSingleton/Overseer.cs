using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Overseer : Static
{
    [SerializeField]
    public static GameObject Sky;
    float timeScaleOriginal;
    public static GameObject RootNode;
    public static List<string> FactionNames;
    private float worldSize = 1800f;

    private static GameObject Saturn;
    private static List<GameObject> Moons = new List<GameObject>();
    private static List<GameObject> AsteroidFields = new List<GameObject>();

    //float timeScale;
    new void Start()
    {
        base.Start();

        // Faction Names
        FactionNames = new List<string>();
        FactionNames.Add("Reds");
        FactionNames.Add("Freemans");
        FactionNames.Add("Looters");
        FactionNames.Add("Greens");


        // Initialize Stuff Above
        timeScaleOriginal = Time.fixedDeltaTime;
        Sky = Resources.Load("Prefabs/SkyPrefab", typeof(GameObject)) as GameObject;
        RootNode = GameObject.FindWithTag("RootNode");
        if(RootNode != null)
        {
            Debug.Log("Found Root Node!");
        }
        CreateSaturnSystem();
        CreatePlanetNodes();
        CreateSky();
        //TODO: AssignPlanetFactions();
        Debug.Log("--OVERSEER LOADING COMPLETE");
    }

    bool CreateSky()
    {
        GameObject sky = Instantiate(Sky, new Vector3(0,-16,0), Quaternion.identity);
        return true;
    }

    void CreateSaturnSystem()
    {
        Saturn = Instantiate((GameObject)Resources.Load("Prefabs/Moon"), new Vector3(0, 0, 0), Quaternion.identity);
        Saturn.transform.localScale += (new Vector3(30,30,30) - Saturn.transform.localScale);
        Saturn.name = "Saturn";
        int numMoons = 25;
        for (int i = 0; i < numMoons; i++)
        {
            float x = (Random.value * worldSize) - (worldSize / 2);
            //float x = ((Random.value * worldSize) - (worldSize / 2))/70f;
            float z = (Random.value * worldSize) - (worldSize / 2);
            if (CheckForReject(x, z))
            {
                GameObject moon = Instantiate((GameObject)Resources.Load("Prefabs/Moon"), new Vector3(x, 0, z), Quaternion.identity);
                moon.name = "Moon" + i;
                moon.GetComponent<Planet>().RandomizeSize();
                Moons.Add(moon);
            }
            else
            {
                i--;
            }
        }
        int numAsteroidFields = 100;
        for (int i = 0; i < numAsteroidFields; i++)
        {
            float x = (Random.value * worldSize) - (worldSize / 2);
            //float x = ((Random.value * worldSize) - (worldSize / 2))/70f;
            float z = (Random.value * worldSize) - (worldSize / 2);
            if (CheckForRejectAsteroids(x, z))
            {
                GameObject asteroidField = Instantiate((GameObject)Resources.Load("Prefabs/AsteroidField"), new Vector3(x, 0, z), Quaternion.identity);
                asteroidField.name = "AsteroidField" + i;
                AsteroidFields.Add(asteroidField);
            }
            else
            {
                i--;
            }
        }
        foreach (var moon in Moons)
        {
            var hitCollidersNear = Physics.OverlapSphere(moon.transform.position, 50);
            var hitCollidersMedium = Physics.OverlapSphere(moon.transform.root.position, 250);
            var hitCollidersFar = Physics.OverlapSphere(moon.transform.position, 625);
            Debug.Log("Near: " + hitCollidersNear.Length);
            Debug.Log("Medi: " + hitCollidersMedium.Length);
            Debug.Log("Far : " + hitCollidersFar.Length);
            foreach (var collider in hitCollidersNear)
            {
                if (collider.gameObject.tag == "StaticInteractive")
                {
                    if (collider.gameObject.name.StartsWith("Moon"))
                    {
                        if (collider.gameObject.transform.root.gameObject != moon)
                        {
                            Destroy(collider.gameObject);
                        }
                    }
                }
            }
            foreach (var collider in hitCollidersMedium)
            {
                if (collider.gameObject.tag == "StaticInteractive")
                {
                    if (collider.transform.root.name.StartsWith("AsteroidField"))
                    {
                        Debug.Log("ADDING NEARBY ASTEROID FIELD TO PLANET");
                        Planet script = moon.GetComponent<Planet>();
                        script.AddAsteroidField(collider.transform.root.gameObject);
                    }
                }
            }
        }
        foreach (var asteroidField in AsteroidFields)
        {
            var hitCollidersNear = Physics.OverlapSphere(asteroidField.transform.position, 50);
            var hitCollidersMedium = Physics.OverlapSphere(asteroidField.transform.position, 250);
            var hitCollidersFar = Physics.OverlapSphere(asteroidField.transform.position, 625);
            foreach (var collider in hitCollidersNear)
            {
                if (collider.gameObject.transform.root.tag == "StaticInteractive")
                {
                    if (collider.gameObject.transform.root.name.StartsWith("AsteroidField"))
                    {
                        if (collider.gameObject.transform.root.gameObject != asteroidField)
                        {
                            Destroy(collider.gameObject.transform.root.gameObject);
                            Debug.Log("DESTROY!");
                        }
                    }
                }
            }
        }
    }

    bool CheckForReject(float x, float z)
    {
        float dist = Mathf.Sqrt((x * x) + (z * z));
        if (dist > (worldSize/2))
        {
            return false;
        }
        /*if (dist < 100f)
        {
            return false;
        }*/
        int slot = (int) ((dist/(worldSize/2))*32);
        // Debug.Log(slot);
        //  0-3 atmosphere
        //  4-7 interveening space
        //  8-12 thin field
        //  13-21 main field
        //  22-23 edge of space
        //  24-30 a new hope
        //  31-32 last few rocks
        //  33+ deep space
        //                  1  -  -  -  5  -  -  - 10  -  -  - 15  -  -  - 20  -  -  - 25  
        int[] map = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 4, 8, 16, 16, 17, 0, 1, 2, 4, 18, 19, 1, 1, 15, 14, 13, 14, 15, 2, 1 };
        if ((Random.value * 100) >= map[slot])
        {
            return false;
        }
        return true;
    }

    bool CheckForRejectAsteroids(float x, float z)
    {
        float dist = Mathf.Sqrt((x * x) + (z * z));
        if (dist > (worldSize / 2))
        {
            return false;
        }
        /*if (dist < 100f)
        {
            return false;
        }*/
        int slot = (int)((dist / (worldSize / 2)) * 32);
        // Debug.Log(slot);
        //  0-3 atmosphere
        //  4-7 interveening space
        //  8-12 thin field
        //  13-21 main field
        //  22-23 edge of space
        //  24-30 a new hope
        //  31-32 last few rocks
        //  33+ deep space
        int[] map = new[] { 0, 0, 0, 0, 0, 1, 2, 2, 4, 4, 4, 4, 4, 16, 16, 16, 16, 19, 17, 12, 21, 18, 19, 1, 1, 15, 14, 13, 14, 15, 2, 1 };
        if ((Random.value * 100) >= map[slot])
        {
            return false;
        }
        return true;
    }

    bool CreatePlanetNodes()
    {
        var planets = GameObject.FindObjectsOfType<Planet>();
        
        foreach(Planet planet in planets)
        {
            GameObject node = new GameObject("Node");
            node.transform.position = planet.transform.position;
            node.transform.SetParent(RootNode.transform);
            //Instantiate(objectToSpawn, planet.transform.position, Quaternion.identity);
            //Planets.Add(planet.gameObject);

            // 
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Mobile.inTime)
        {
            Mobile.inTime = false;
            Static.inTime = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Mobile.inTime = true;
            Static.inTime = true;
        }
    }
}
