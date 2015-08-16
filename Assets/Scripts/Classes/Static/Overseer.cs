using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Overseer : Static
{
    float timeScaleOriginal;
    public static List<GameObject> Planets;
    public static List<Transform> PlanetNodes;
    public static GameObject RootNode;
    public static List<string> FactionNames;

    //float timeScale;
    new void Start()
    {
        base.Start();

        // Faction Names
        FactionNames = new List<string>();
        FactionNames.Add("Reds");
        FactionNames.Add("Freemans");
        FactionNames.Add("Loters");
        FactionNames.Add("Greens");


        // Initialize Stuff Above
        timeScaleOriginal = Time.fixedDeltaTime;
        Debug.Log("-NOTE: OVERSEER LOADING COMPLETE");
        Planets = new List<GameObject>();
        RootNode = GameObject.FindWithTag("RootNode");
        if(RootNode != null)
        {
            Debug.Log("Found Root Node!");
        }
        CreatePlanetNodes();
        //TODO: AssignPlanetFactions();
    }

    bool CreatePlanetNodes()
    {
        var planets = GameObject.FindObjectsOfType<Planet>();

        Debug.Log(planets.Length);
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
