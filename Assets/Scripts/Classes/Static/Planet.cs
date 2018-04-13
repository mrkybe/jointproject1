using System;
using System.Collections.Generic;
using Assets.Scripts.Classes.Helper;
using Assets.Scripts.Classes.Helper.ShipInternals;
using Assets.Scripts.Classes.WorldSingleton;
using UnityEngine;

namespace Assets.Scripts.Classes.Static {
    /// <summary>
    /// A Base that belongs to a faction.  Sends out ships and trades resources.
    /// </summary>
    public partial class Planet : Static
    {
        [SerializeField]
        private List<GameObject> WorkerShips;

        [SerializeField]
        public Faction Faction;

        [SerializeField]
        public string MyName;

        [SerializeField]
        private Timer TimeToSpawn;

        [SerializeField]
        public float Mass = 1;

        [SerializeField]
        public float Radius = 1;

        [SerializeField]
        int MaxFriends;

        [SerializeField]
        private List<Building> myBuildings = new List<Building>();

        [SerializeField]
        public bool hasGravity;

        [SerializeField]
        public static List<Planet> listOfPlanetObjects = new List<Planet>();

        [SerializeField]
        private CargoHold myStorage;

        [SerializeField]
        private CargoHold reservedStorage;

        private Faction faction;

        private bool AmPaused = false;

        void Awake()
        {
            WorkerShips = new List<GameObject>();

            listOfPlanetObjects.Add(this);

            TimeToSpawn = gameObject.AddComponent<Timer>();
            TimeToSpawn.SetTimer(1);
            TimeToSpawn.Loop(true);
        

            myStorage = new CargoHold(this, 50000);
            reservedStorage = new CargoHold(this, 50000);

            SetupBuildings();
            PlanetBTSetup();
            //behaviorTree; // Was causing compile errors
        }

        /// <summary>
        /// Set the faction of the Planet.
        /// </summary>
        /// <param name="f"></param>
        public void SetFaction(Faction f)
        {
            if (Faction != null)
            {
                Faction.Unown(this);
            }

            Faction = f;
            f.Own(this);
            
            MeshRenderer mr = transform.GetChild(0).GetComponent<MeshRenderer>();
            Color glowColor = f.ColorPrimary;
            glowColor.a = 0.5f;
            mr.material.color = glowColor;

            transform.GetChild(2).GetChild(0).GetComponent<TextMesh>().text = this.MyName;
            transform.GetChild(2).GetChild(1).GetComponent<TextMesh>().text = this.Faction.Name;
        }

        /// <summary>
        /// Creates a random assortment of buildings on the Planet.
        /// </summary>
        public void SetupBuildings()
        {
            System.Random random = new System.Random(GetInstanceID());
            int enviromentalStartingCount = random.Next(2) + 3;
            int industrialStartingCount = random.Next(2) + 1;
            // Add enviromental buildings
            for (int i = 0; i < enviromentalStartingCount; i++)
            {
                myBuildings.Add(Building.BasicEnviroments[random.Next(Building.BasicEnviroments.Length)]());
            }
            // Add industrial buildings
            for (int i = 0; i < industrialStartingCount; i++)
            {
                myBuildings.Add(Building.BasicIndustry[random.Next(Building.BasicIndustry.Length)]());
            }
            myBuildings.Sort((a,b) => string.CompareOrdinal(a.Name, b.Name));
            TickBuildings(random.Next(25) + 25);
        }

        /// <summary>
        /// Randomizes the Size of the Planet.
        /// </summary>
        public void RandomizeSize()
        {
            System.Random random = new System.Random(GetInstanceID());

            /*Radius = (float)(random.NextDouble() * 13) + 2f;
        Mass = (float)(4 * Math.PI * Math.Pow(Radius / 2, 3));*/
            Radius = (float)(random.NextDouble() * 1) + 2f;
            Mass = (float)(4 * Math.PI * Math.Pow(Radius / 2, 3));

            transform.GetChild(0).transform.localScale = new Vector3(3,1,3);
            transform.GetChild(1).transform.localScale = new Vector3(Radius * 2f, Radius * 2f, Radius * 2f);
        }
	
        // Update is called once per frame
        private void Update ()
        {
            transform.Rotate(Vector3.up, Time.deltaTime * -1f);
        }

        private new void FixedUpdate()
        {

        }

        private void TickBuildings(int multiplier = 1)
        {
            for (int i = 0; i < multiplier; i++)
            {
                foreach (var building in myBuildings)
                {
                    building.Tick(myStorage);
                }
            }
        }

        /// <summary>
        /// Returns the Cargohold of the Planet.
        /// </summary>
        public CargoHold GetCargoHold
        {
            get { return myStorage; }
        }

        /// <summary>
        /// Returns the Cargohold of the Planet for resources that are already committed.
        /// </summary>
        public CargoHold GetReserveCargoHold
        {
            get { return reservedStorage; }
        }

        /// <summary>
        /// Returns a list of the buildings in the planet as a string.
        /// </summary>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public string BuildingsToString(string seperator = "\n")
        {
            string BuildingsNamed = "";
            foreach (var building in myBuildings)
            {
                BuildingsNamed += building.ToString() + seperator;
            }
            return BuildingsNamed;
        }

        /// <summary>
        /// Sets the name of the Planet.
        /// </summary>
        /// <param name="val"></param>
        public void SetName(string val)
        {
            MyName = val;
        }


        /// <summary>
        /// Takes a step forward in time.  Called by Overseer.cs.  Not every planet has its Tick called at the same time.  You have been warned.
        /// </summary>
        public void Tick()
        {
            TickBuildings();
        }

        /// <summary>
        /// Compares Planets.
        /// </summary>
        public class PlanetComparer : IComparer<Planet>
        {
            private Planet origin;

            public PlanetComparer(Planet origin)
            {
                this.origin = origin;
            }

            public int CompareClosest(Planet x, Planet y)
            {
                Vector3 xPos = x.transform.position;
                Vector3 yPos = y.transform.position;
                Vector3 originPos = origin.transform.position;
                float xDist = Vector3.Distance(originPos, xPos);
                float yDist = Vector3.Distance(originPos, yPos);
                if (xDist < yDist)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }

            /// <summary>
            /// Returns which planet is closer to the origin planet.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Compare(Planet x, Planet y)
            {
                return CompareClosest(x, y);
            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return base.ToString();
            }
        }

        protected new void OnDestroy()
        {
            base.OnDestroy();
            listOfPlanetObjects.Remove(this);
        }

        public override void OvermapPause()
        {
            AmPaused = true;
        }

        public override void OvermapUnpause()
        {
            AmPaused = false;
        }
    }
}
