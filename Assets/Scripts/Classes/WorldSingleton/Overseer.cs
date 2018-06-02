using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Classes.Helper;
using Assets.Scripts.Classes.Helper.Pilot;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.Mobile;
using BehaviorDesigner.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Classes.WorldSingleton
{
    /// <summary>
    /// The Singleton that manages the systems in the game.  Organized into partial classes.
    ///     Overseer.cs  - Main,
    ///     Market.cs    - Economy,
    ///     Diplomacy.cs - Factions
    /// </summary>
	/// 
	/// 
	/// 
    

	public enum GameState
	{
		GameOver = 0,
		InOverMap = 1,
		InCombat = 2,
	    UI = 3
	}



	public partial class Overseer : MonoBehaviour
    {
		
        [SerializeField]
        public static GameObject Sky;
        
        public static GameObject[] Explosions;

        public DateTime InUniverseDateTime;

        private float timeScaleOriginal;
        public bool MatchOrdersAuto = true;
        public static GameObject RootNode;
        [SerializeField]
        public float worldSize = 1600f;
        [SerializeField]
        int NumberOfAsteroidFields = 250;
        public float planetTickFrequency = 10;

        private static GameObject Saturn;
        private static List<GameObject> Moons = new List<GameObject>();
        private static List<GameObject> AsteroidFields = new List<GameObject>();
        [SerializeField]
        private static List<Spaceship> OutskirtShips = new List<Spaceship>();
        public static Overseer Main;
        private GameObject PirateShip;
        public GameState gameState = GameState.InOverMap;

        //float timeScale;
        private void Awake()
        {
			Time.timeScale = 1f;
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
            PirateShip = (GameObject)Resources.Load("Prefabs/AI_ship");
            RootNode = GameObject.FindWithTag("RootNode");

			gameState = GameState.InOverMap;

            CreateResourceTypes();
            CreateFactions();
            CreateSaturnSystem();
            CreatePlanetNodes();
            CreateSky();
            CreateMarket();
            LoadExplosions();

            InUniverseDateTime = new DateTime(2087,1,1,0,0,0);

            InvokeRepeating("TickPlanets", 1f, planetTickFrequency);
            //Invoke("TickPlanets", 1f);
            InvokeRepeating("ManagePirateCount", 1f, 1f);
            Invoke("InitializeDelayed", 1f);
        }

        public void InitializeDelayed()
        {
            SetBehaviorManagerTickrate(gameState);
        }

        public void SetBehaviorManagerTickrate(GameState st)
        {
            if (st == GameState.InOverMap)
            {
                BehaviorManager.instance.UpdateIntervalSeconds = 0.2f;
                BehaviorManager.instance.MaxTaskExecutionsPerTick = 1;
            }
            else if (st == GameState.InCombat)
            {
                BehaviorManager.instance.UpdateIntervalSeconds = 0;
                BehaviorManager.instance.MaxTaskExecutionsPerTick = 0;
            }
        }

        private Queue<GameObject> listOfExplosionObjects = new Queue<GameObject>();

		public void DoExplosion(Vector3 pos, int layer, float scale = 0.01f)
        {
            int which = Random.Range(0, Explosions.Length);
            pos = pos + new Vector3(0, 1, 0);
            GameObject exp = Instantiate(Explosions[which], pos, Quaternion.identity);
			exp.layer = layer;
			Transform expTrans = exp.transform;
			if (exp.transform.childCount > 0) 
			{
				Transform[] children = expTrans.GetComponentsInChildren<Transform> ();
				for(int i = 0; i < children.Length; i++)
				{
					children [i].gameObject.layer = layer;
				}
			}
            exp.transform.localScale = Vector3.one * scale;
            listOfExplosionObjects.Enqueue(exp);
            Invoke("DestroyLastExplosion",1);
        }

        private void DestroyLastExplosion()
        {
            GameObject exp = listOfExplosionObjects.Dequeue();
            Destroy(exp);
        }
        
        private void LoadExplosions()
        {
            int[] numsToLoad = {3, 5, 7, 8, 9, 10, 12, 13};
            Explosions = new GameObject[numsToLoad.Length];
            for (int i = 0; i < numsToLoad.Length; i++)
            {
                string root = "Prefabs/PC/";
                string name = "Explosion" + numsToLoad[i];
                string path = Path.Combine(root, name);
                Explosions[i] = (GameObject)Resources.Load(path);
            }
        }

        public int ClaimBountyOn(Spaceship ship)
        {
            int totalBounty = 0;
            foreach (Faction f in Factions)
            {
                totalBounty += f.ClaimBountyOn(ship);
            }
            return totalBounty;
        }

        private new void Start()
        {
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

        private bool CreateSky()
        {
            GameObject sky = Instantiate(Sky, new Vector3(0,-16,0), Quaternion.identity);
            return true;
        }

        private Vector3 PolarCoordinates(float rad, float distance)
        {
            float x = Mathf.Sin(2 * rad * Mathf.PI) * distance;
            float z = Mathf.Cos(2 * rad * Mathf.PI) * distance;
            return new Vector3(x, 0, z);
        }

        private Planet FindClosest(List<Planet> list, Planet current)
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

        private Vector3 FindClosest(List<Vector3> list, Vector3 current)
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

        private Vector3 GetBestCandidate(List<Vector3> samples, int numCandidates)
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

        private List<Vector3> GenerateMoonPositions(int numMoons, int numCandidates, List<Vector3> alsoAvoid = null)
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

        private void CreateSaturnSystem()
        {
            Saturn = Instantiate((GameObject)Resources.Load("Prefabs/Saturn"), new Vector3(0, 0, 0), Quaternion.identity);
            Saturn.transform.localScale += (new Vector3(30,30,30) - Saturn.transform.localScale);
            Saturn.name = "Saturn";
            int numMoons = 50;
            Queue<string> moon_names = new Queue<string>(ListOfSaturnMoonNames());
            List<Vector3> moon_positions = GenerateMoonPositions(numMoons, 15);
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

            List<Vector3> asteroid_positions = GenerateMoonPositions(NumberOfAsteroidFields, 2, moon_positions);
            for (int i = 0; i < NumberOfAsteroidFields; i++)
            {
                GameObject asteroidField = Instantiate((GameObject)Resources.Load("Prefabs/AsteroidField"), asteroid_positions[i], Quaternion.identity);
                asteroidField.name = "AsteroidField" + i;
                AsteroidFields.Add(asteroidField);
            }
        }

        private bool CreatePlanetNodes()
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
            int numPlanets = Moons.Count;
            int planetsToDoPerIteration = 10;
            float timeBetweenIterations = ((((float)numPlanets / planetsToDoPerIteration)) / planetTickFrequency) / 2f;
            if (IsOvermapPaused())
            {
                yield break;
            }
            foreach (Planet p in Planet.listOfPlanetObjects)
            {
                if (counter > planetsToDoPerIteration)
                {
                    counter = 0;
                    yield return new WaitForSeconds(timeBetweenIterations);
                }
                else
                {
                    p.Tick();
                    counter++;
                }
            }
            MatchOrders();
        }

        public void ManagePirateCount()
        {
            if (IsOvermapPaused())
            {
                return;
            }
            OutskirtShips.RemoveAll(x => x == null || x.Alive == false);
            var PirateShips = OutskirtShips.Where(x => x.Pilot.Faction == GetFaction("Pirates"));
            var BountyHunters = OutskirtShips.Where(x => x.Pilot.Faction == GetFaction("Independent"));
            var RobotShips = OutskirtShips.Where(x => x.Pilot.Faction == GetFaction("Robots"));
            if (PirateShips.Count() < 5)
            {
                SpawnSpaceshipOnOutskirts();
                OutskirtShipCounter++;
            }
            if (BountyHunters.Count() < 5)
            {
                SpawnSpaceshipOnOutskirts(GetFaction("Independent"));
                OutskirtShipCounter++;
            }
            if (RobotShips.Count() < 5)
            {
                SpawnSpaceshipOnOutskirts(GetFaction("Robots"));
                OutskirtShipCounter++;
            }
        }

        /// <summary>
        /// Allows the Unity Editor to call this method.  Don't use this unless you know what you're doing.
        /// </summary>
        /// <param name="typename"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private int OutskirtShipCounter = 0;
        public Spaceship SpawnSpaceshipOnOutskirts(Faction faction = null)
        {
            if (faction == null)
            {
                faction = GetFaction("Pirates");
            }
            Vector3 position = PolarCoordinates((float)(Math.PI * 2 * Random.value), worldSize / 1.75f);
            Vector2 offset = Random.insideUnitCircle.normalized * (this.transform.localScale.magnitude + 1);
            Vector3 offset3d = new Vector3(offset.x, 0, offset.y);

            Quaternion shipRotation = Quaternion.LookRotation(offset3d, Vector3.up);
            GameObject ship = null;

            ship = Instantiate(PirateShip, position + offset3d, shipRotation);
            AI_Patrol pilot = ship.GetComponent<AI_Patrol>();
            Spaceship shipScript = ship.GetComponent<Spaceship>();
            ship.name = "S_" + faction.Name + "_" + OutskirtShipCounter;
            shipScript.Pilot.Faction = faction;

            if (faction.Name == "Pirates" || faction.Name == "Independent")
            {
                pilot.StartPirate();
            }
            else if (faction.Name == "Robots")
            {
                pilot.StartScrapper();
            }

            OutskirtShips.Add(shipScript);
            OutskirtShipCounter++;
            return shipScript;
        }

        // Update is called once per frame
        private bool pausekey = false;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P) && !pausekey)
            {
                PauseOvermap();
                pausekey = true;
            }
            else if (Input.GetKeyDown(KeyCode.P) && pausekey)
            {
                UnpauseOvermap();
                pausekey = false;
            }

            if (gameState == GameState.InOverMap && !IsOvermapPaused())
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    Time.timeScale = 4.0f;
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    Time.timeScale = 1.0f;
                }
            }

            if (gameState == GameState.InOverMap)
            {
                InUniverseDateTime = InUniverseDateTime.AddHours(Time.deltaTime);
            }
        }
        
        private int overmap_pause_count = 0;
        public bool IsOvermapPaused()
        {
            return overmap_pause_count > 0;
        }

        public void PauseOvermap()
        {
            if (!IsOvermapPaused())
            {
                Time.timeScale = 0.0f;
                foreach (Spaceship ship in FindObjectsOfType(typeof(Spaceship)))
                {
                    ship.Pause();
                }
                foreach (Static.Static st in Static.Static.listOfStaticObjects)
                {
                    st.OvermapPause();
                }
                overmap_pause_count++;
            }
            else
            {
                overmap_pause_count++;
            }
        }

        public void UnpauseOvermap()
        {
            if (overmap_pause_count == 1)
            {
                Time.timeScale = 1.0f;
                foreach (Spaceship ship in FindObjectsOfType(typeof(Spaceship)))
                {
                    ship.Unpause();
                }
                foreach (Static.Static st in Static.Static.listOfStaticObjects)
                {
                    st.OvermapUnpause();
                }
                overmap_pause_count--;
            }
            else
            {
                overmap_pause_count--;
            }
        }

        private bool cheatMove = false;
        public void ToggleCheatMoveShips()
        {
            if (cheatMove)
            {
                cheatMove = false;
                foreach (Spaceship s in FindObjectsOfType(typeof(Spaceship)))
                {
                    s.CheatSpeed = cheatMove;
                }
            }
            else
            {
                cheatMove = true;
                foreach (Spaceship s in FindObjectsOfType(typeof(Spaceship)))
                {
                    s.CheatSpeed = cheatMove;
                }
            }
        }

        private List<string> ListOfSaturnMoonNames()
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
