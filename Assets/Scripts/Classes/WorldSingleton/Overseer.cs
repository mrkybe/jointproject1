using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.Mobile;
using BehaviorDesigner.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Classes.WorldSingleton
{
    /// <summary>
    /// The Singleton that manages the systems in the game.  Organized into partial classes.
    ///     Overseer.cs  - Main
    ///     Market.cs    - Economy
    ///     Diplomacy.cs - Factions
    /// </summary>
    public partial class Overseer : Static.Static
    {
        [SerializeField]
        public static GameObject Sky;
        float timeScaleOriginal;
        public bool MatchOrdersAuto = true;
        public static GameObject RootNode;
        private float worldSize = 800f;

        private static GameObject Saturn;
        private static List<GameObject> Moons = new List<GameObject>();
        private static List<GameObject> AsteroidFields = new List<GameObject>();
        public static Overseer Main;

        //float timeScale;
        void Awake()
        {
            if (Main == null)
            {
                Main = this;
            }
            else if (Main == this)
            {
                throw new Exception("Overseer Object Already Called Start() Once Please Fix");
            }
            else
            {
                throw new Exception("Overseer Object Already Created Please Fix");
            }
            //Debug.unityLogger.logEnabled = false; 
            Sky = Resources.Load("Prefabs/SkyPrefab", typeof(GameObject)) as GameObject;
            RootNode = GameObject.FindWithTag("RootNode");

            CreateFactions();
            CreateSaturnSystem();
            CreatePlanetNodes();
            CreateSky();
            CreateMarket();

            InvokeRepeating("TickPlanets", 1f, 1f);
        }

        new void Start()
        {
            base.Start();
            // Initialize Stuff Above
            timeScaleOriginal = Time.fixedDeltaTime;
            if(RootNode != null)
            {
                //Debug.Log("Found Root Node!");
            }
            StartMatchingOrders();
            //TODO: AssignPlanetFactions();
            //Debug.Log("--OVERSEER LOADING COMPLETE");

            BehaviorManager.instance.UpdateIntervalSeconds = 0.2f;
            BehaviorManager.instance.MaxTaskExecutionsPerTick = 1;
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

        Planet FindClosest(List<Planet> list, Planet current)
        {
            Planet result = list[0];
            float best = Vector3.Distance(list[0].transform.position, current.transform.position);
            foreach (Planet pos in list)
            {
                float d = Vector3.Distance(pos.transform.position, current.transform.position);
                if (d < best)
                {
                    best = d;
                    result = pos;
                }
            }
            return result;
        }

        Vector3 FindClosest(List<Vector3> list, Vector3 current)
        {
            Vector3 result = list[0];
            float best = Vector3.Distance(list[0], current);
            foreach (Vector3 pos in list)
            {
                float d = Vector3.Distance(pos, current);
                if (d < best)
                {
                    best = d;
                    result = pos;
                }
            }
            return result;
        }

        Vector3 GetBestCandidate(List<Vector3> samples, int numCandidates)
        {
            Vector3 bestCandidate = Vector3.zero;
            float bestDistance = 0;
            for (var i = 0; i < numCandidates; ++i)
            {
                Vector3 c = PolarCoordinates(Random.value, 20f + Random.value * ((worldSize/2) - 100f));

                float d = Vector3.Distance(FindClosest(samples, c), c);
                if (d > bestDistance)
                {
                    bestDistance = d;
                    bestCandidate = c;
                }
            }

            return bestCandidate;
        }

        List<Vector3> GenerateMoonPositions(int numMoons, int numCandidates, List<Vector3> alsoAvoid = null)
        {
            List<Vector3> activeSamples = new List<Vector3>();
            if (alsoAvoid != null)
            {
                activeSamples.AddRange(alsoAvoid);
            }
            else
            {
                activeSamples.Add(Vector3.zero);
            }
            List<Vector3> result = new List<Vector3>();
            while (result.Count < (numMoons+1))
            {
                Vector3 sample = GetBestCandidate(activeSamples, numCandidates);
                activeSamples.Add(sample);
                result.Add(sample);
            }
            return result;
        }

        void CreateSaturnSystem()
        {
            Saturn = Instantiate((GameObject)Resources.Load("Prefabs/Saturn"), new Vector3(0, 0, 0), Quaternion.identity);
            Saturn.transform.localScale += (new Vector3(30,30,30) - Saturn.transform.localScale);
            Saturn.name = "Saturn";
            int numMoons = 50;
            Queue<string> moon_names = new Queue<string>(ListOfSaturnMoonNames());
            List<Vector3> moon_positions = GenerateMoonPositions(numMoons, 10);
            List<Planet> moon_scripts = new List<Planet>();
            for (int i = 0; i < (numMoons); i++)
            {
                GameObject moon = Instantiate((GameObject)Resources.Load("Prefabs/Moon"), moon_positions[i], Quaternion.identity);
                var script = moon.GetComponent<Planet>();
                script.RandomizeSize();
                string name = "Moon" + i;
                moon.name = name;
                if (moon_names.Count > 0)
                {
                    name = moon_names.Dequeue();
                    script.SetName(name);
                }
                Moons.Add(moon);
                moon_scripts.Add(script);
            }

            int k = MainFactions.Count;
            int num_nearest_candidates = 15;
            List<Planet> claimed = new List<Planet>();
            List<Planet> motherPlanets = new List<Planet>();
            List<Planet> unclaimed = new List<Planet>();
            unclaimed.AddRange(moon_scripts);
            for (int i = 0; i < k; i++)
            {
                //Vector3 closest = FindClosest(moon_positions, PolarCoordinates((float)((Math.PI * 2) / (k+1)), worldSize));
                //Planet moon = moon_scripts.First(x => x.transform.position == closest);
                Planet moon = moon_scripts[Random.Range(0, moon_scripts.Count)];
                if (moon != null)
                {
                    moon.SetFaction(MainFactions[i]);
                    claimed.Add(moon);
                    motherPlanets.Add(moon);
                    unclaimed.Remove(moon);
                }
            }
            int iter = 0;
            while (unclaimed.Count > 0)
            {
                Planet new_owner = motherPlanets[Random.Range(0, k)];
                List<Planet> candidates = new List<Planet>();
                for (int i = 0; i < num_nearest_candidates && i < unclaimed.Count; i++)
                {
                    int r = Random.Range(0, unclaimed.Count);
                    Planet x = unclaimed[r];
                    unclaimed.RemoveAt(r);
                    candidates.Add(x);
                }
                Planet closest = FindClosest(candidates, new_owner);
                candidates.Remove(closest);
                closest.SetFaction(new_owner.Faction);
                unclaimed.AddRange(candidates);
                iter++;
            }

            int numAsteroidFields = 25;
            List<Vector3> asteroid_positions = GenerateMoonPositions(numAsteroidFields, 5, moon_positions);
            for (int i = 0; i < numAsteroidFields; i++)
            {
                GameObject asteroidField = Instantiate((GameObject)Resources.Load("Prefabs/AsteroidField"), asteroid_positions[i], Quaternion.identity);
                asteroidField.name = "AsteroidField" + i;
                AsteroidFields.Add(asteroidField);
            }
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

        private void TickPlanets()
        {
            StartCoroutine(TickPlanetsCoroutine());
        }

        private IEnumerator TickPlanetsCoroutine()
        {
            int counter = 0;
            int planets_to_do_per_iteration = 10;
            foreach (Planet p in Planet.listOfPlanetObjects)
            {
                if (counter > planets_to_do_per_iteration)
                {
                    counter = 0;
                    yield return null;
                }
                else
                {
                    p.Tick();
                    counter++;
                }
            }
            MatchOrders();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && Spaceship.inTime)
            {
                Spaceship.inTime = false;
                Static.Static.inTime = false;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Spaceship.inTime = true;
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
