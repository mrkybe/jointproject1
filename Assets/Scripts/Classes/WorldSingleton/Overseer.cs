using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Classes.WorldSingleton
{
    public partial class Overseer : Static.Static
    {
        [SerializeField]
        public static GameObject Sky;
        float timeScaleOriginal;
        public static GameObject RootNode;
        private float worldSize = 1800f;

        private static GameObject Saturn;
        private static List<GameObject> Moons = new List<GameObject>();
        private static List<GameObject> AsteroidFields = new List<GameObject>();

        //float timeScale;
        new void Start()
        {
            base.Start();
            // Initialize Stuff Above
            timeScaleOriginal = Time.fixedDeltaTime;
            Sky = Resources.Load("Prefabs/SkyPrefab", typeof(GameObject)) as GameObject;
            RootNode = GameObject.FindWithTag("RootNode");
            if(RootNode != null)
            {
                //Debug.Log("Found Root Node!");
            }
            CreateFactions();
            CreateSaturnSystem();
            CreatePlanetNodes();
            CreateSky();
            //TODO: AssignPlanetFactions();
            //Debug.Log("--OVERSEER LOADING COMPLETE");
        }

        bool CreateSky()
        {
            GameObject sky = Instantiate(Sky, new Vector3(0,-16,0), Quaternion.identity);
            return true;
        }

        Vector3 PolarCoordinates(float rad, float distance)
        {
            float x = Mathf.Sin(2 * rad * Mathf.PI) * distance;
            float z = Mathf.Cos(2 * rad * Mathf.PI) * distance;
            return new Vector3(x, 0, z);
        }

        void CreateSaturnSystem()
        {
            Saturn = Instantiate((GameObject)Resources.Load("Prefabs/Moon"), new Vector3(0, 0, 0), Quaternion.identity);
            Saturn.transform.localScale += (new Vector3(30,30,30) - Saturn.transform.localScale);
            Saturn.name = "Saturn";
            int numMoons = 50;
            float minSaturnDistance = 75f;
            Queue<string> moon_names = new Queue<string>(ListOfSaturnMoonNames());
            for (int i = 0; i < (numMoons); i++)
            {
                float stepCount = ((worldSize / 2) - minSaturnDistance  )/numMoons;
                var coords = PolarCoordinates(Random.value, minSaturnDistance + i * stepCount);
                float x = coords.x;
                float z = coords.z;

                GameObject moon = Instantiate((GameObject)Resources.Load("Prefabs/Moon"), new Vector3(x, 0, z), Quaternion.identity);
                moon.name = "Moon" + i;
                var script = moon.GetComponent<Planet>();
                script.RandomizeSize();
                string name = moon_names.Dequeue();
                script.SetName(name);
                script.SetFaction(GetRandomFaction());
                Moons.Add(moon);
            }
            int numAsteroidFields = 1000;
            for (int i = 0; i < numAsteroidFields; i++)
            {
                //float x = (Random.value * worldSize) - (worldSize / 2);
                //float x = ((Random.value * worldSize) - (worldSize / 2))/70f;
                //float z = (Random.value * worldSize) - (worldSize / 2);
                float stepCount = ((worldSize / 2) - minSaturnDistance) / numAsteroidFields;
                var coords = PolarCoordinates(Random.value, minSaturnDistance + Random.value * ((worldSize / 2) - minSaturnDistance));
                float x = coords.x;
                float z = coords.z;
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
            List<GameObject> MoonsToDestroy = new List<GameObject>();
            List<GameObject> AsteroidsToDestroy = new List<GameObject>();
            foreach (var moon in Moons)
            {
                var hitCollidersNear = Physics.OverlapSphere(moon.transform.position, 75);
                //var hitCollidersMedium = Physics.OverlapSphere(moon.transform.root.position, 250);
                //var hitCollidersFar = Physics.OverlapSphere(moon.transform.position, 625);
                //Debug.Log("Near: " + hitCollidersNear.Length);
                //Debug.Log("Medi: " + hitCollidersMedium.Length);
                //Debug.Log("Far : " + hitCollidersFar.Length);
                if (MoonsToDestroy.Contains(moon))
                {
                    continue;
                }
                foreach (var collider in hitCollidersNear)
                {
                    if (collider.gameObject.tag == "StaticInteractive")
                    {
                        if (collider.gameObject.name.StartsWith("Moon"))
                        {
                            if (collider.gameObject.transform.root.gameObject != moon)
                            {
                                MoonsToDestroy.Add(collider.gameObject.transform.root.gameObject);
                            }
                        }
                        else if (collider.gameObject.name.StartsWith("AsteroidField"))
                        {
                            float distance = (collider.gameObject.transform.position - moon.transform.position).magnitude;
                            if (distance < 35f)
                            {
                                AsteroidsToDestroy.Add(collider.gameObject.transform.root.gameObject);
                            }
                        }
                    }
                }
            }
            foreach (var moon in MoonsToDestroy)
            {
                Moons.Remove(moon);
                Destroy(moon);
            }
            foreach (var asteroid in AsteroidsToDestroy)
            {
                AsteroidFields.Remove(asteroid);
                Destroy(asteroid);
            }
            Debug.Log("ASTEROID FIELDS COUNT:" + AsteroidFields.Count);
            Debug.Log("MOONS COUNT:" + Moons.Count);
            AsteroidsToDestroy.Clear();
            foreach (var asteroidField in AsteroidFields)
            {
                var hitCollidersNear = Physics.OverlapSphere(asteroidField.transform.position, 9);
                //var hitCollidersMedium = Physics.OverlapSphere(asteroidField.transform.position, 250);
                //var hitCollidersFar = Physics.OverlapSphere(asteroidField.transform.position, 625);
                if (AsteroidsToDestroy.Contains(asteroidField))
                {
                    continue;
                }
                foreach (var collider in hitCollidersNear)
                {
                    if (collider.gameObject.transform.root.tag == "StaticInteractive")
                    {
                        if (collider.gameObject.transform.root.name.StartsWith("AsteroidField"))
                        {
                            if (collider.gameObject.transform.root.gameObject != asteroidField)
                            {
                                AsteroidsToDestroy.Add(collider.gameObject.transform.root.gameObject);
                            }
                        }
                    }
                }
            }
            foreach (var asteroid in AsteroidsToDestroy)
            {
                AsteroidFields.Remove(asteroid);
                Destroy(asteroid);
            }

            int counter = 0;
            foreach (var moon in Moons)
            {
                moon.name = "Moon" + counter;
                counter += 1;
            }
            counter = 0;
            foreach (var asteroidField in AsteroidFields)
            {
                asteroidField.name = "AsteroidField" + counter;
                counter += 1;
            }
        }

        bool CheckForReject(float x, float z)
        {
            float dist = Mathf.Sqrt((x * x) + (z * z));
            /*if (dist > (worldSize/2))
            {
                return false;
            }*/
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
            if ((Random.value * 50) <= map[slot])
            {
                return true;
            }
            return false;
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
                Static.Static.inTime = false;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Mobile.inTime = true;
                Static.Static.inTime = true;
            }
        }

        List<string> ListOfSaturnMoonNames()
        {
            return new List<string>
                   {
                       "Pan",
                       "Daphnis",
                       "Atlas",
                       "Prometheus",
                       "Pandora",
                       "Epimetheus",
                       "Janus",
                       "Aegaeon",
                       "Mimas",
                       "Methone",
                       "Anthe",
                       "Pallene",
                       "Enceladus",
                       "Tethys",
                       "Telesto",
                       "Calypso",
                       "Dione",
                       "Helene",
                       "Polydeuces",
                       "Rhea",
                       "Titan",
                       "Hyperion",
                       "Iapetus",
                       "Kiviuq",
                       "Ijiraq",
                       "Phoebe",
                       "Paaliaq",
                       "Skathi",
                       "Albiorix",
                       "S/2007 S 2",
                       "Bebhionn",
                       "Erriapo",
                       "Skoll",
                       "Siarnaq",
                       "Tarqeq",
                       "S/2004 S 13",
                       "Greip",
                       "Hyrrokkin",
                       "Jarnsaxa",
                       "Tarvos",
                       "Mundilfari",
                       "S/2006 S 1",
                       "S/2004 S 17",
                       "Bergelmir",
                       "Narvi",
                       "Suttungr",
                       "Hati",
                       "S/2004 S 12",
                       "Farbauti",
                       "Thrymr",
                       "Aegir",
                       "S/2007 S 3",
                       "Bestla",
                       "S/2004 S 7",
                       "S/2006 S 3",
                       "Fenrir",
                       "Surtu",
                       "Kari",
                       "Ymir",
                       "Loge",
                       "Fornjot"
                   };
        }
    }
}
