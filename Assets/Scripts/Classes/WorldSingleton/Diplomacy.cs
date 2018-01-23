using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Classes.WorldSingleton
{
    /// <summary>
    /// The part of the singleton that manages inter-faction relationships.
    /// </summary>
    public partial class Overseer : Static.Static
    {
        [SerializeField]
        public static List<string> FactionNamesList;

        [SerializeField]
        public static List<Faction> Factions;

        [SerializeField]
        public static List<FactionLink> Links;

        private void InitializeFactions()
        {
            FactionNamesList = new List<string>
            {
                "Communist",
                "Libertarian",
                "Freedom",
                "Duty",
                "Independent",
                "Pirates"
            };
            Factions = new List<Faction>();
            Links = new List<FactionLink>();
        }

        private Faction GetRandomFaction()
        {
            int numFactions = Factions.Count;
            // -1 to exclude pirates
            int val = Random.Range(0, numFactions-1);
            return Factions[val];
        }

        private void CreateFactions()
        {
            InitializeFactions();
            int count = 0;
            foreach (var name in FactionNamesList)
            {
                Faction f = new Faction(name);
                float frac = count / (FactionNamesList.Count * 2f);
                f.ColorPrimary = Color.HSVToRGB(frac, 0.8f, 1f);
                f.ColorSecondary = Color.HSVToRGB(frac + 0.5f, 0.8f, 1f);
                Factions.Add(f);
                count++;
            }

            FactionLinkEqualityComparer comparer = new FactionLinkEqualityComparer();
            foreach (var faction_a in Factions)
            {
                foreach (var faction_b in Factions)
                {
                    if (faction_a == faction_b)
                    {
                        continue;
                    }
                    FactionLink link = new FactionLink(faction_a, faction_b);
                    if (!Links.Exists(e => comparer.Equals(e, link)))
                    {
                        faction_a.MyLinks.Add(link);
                        faction_b.MyLinks.Add(link);
                        Links.Add(link);
                    }
                }
            }

            MakePiratesHostileWithAll();

            //Debug.Log("Created Factions!");

            foreach (var link in Links)
            {
                //Debug.Log(link.ToString());
            }
        }

        private void MakePiratesHostileWithAll()
        {
            Faction f = GetFaction("Pirates");
            foreach (var link in f.MyLinks)
            {
                link.Friendlyness = -100f;
            }
        }

        public Faction GetFaction(string name)
        {
            foreach (Faction f in Factions)
            {
                if (f.Name == name)
                {
                    return f;
                }
            }
            return null;
        }

        public void ResolveShipCombat(AI_Patrol attacker, AI_Patrol defender)
        {
            Spaceship attacker_ship = attacker.GetShip();
            Spaceship defender_ship = defender.GetShip();
            IEnumerable<Spaceship> ship1_allies = attacker_ship.GetShipsInInteractionRange().Where(x => x.Faction == attacker_ship.Faction);
            IEnumerable<Spaceship> ship2_allies = defender_ship.GetShipsInInteractionRange().Where(x => x.Faction == defender_ship.Faction);

            int attacker_power = attacker_ship.PowerLevel + ship1_allies.Sum(x => x.PowerLevel);
            int defender_power = defender_ship.PowerLevel + ship2_allies.Sum(x => x.PowerLevel);

            int attacker_roll = (int)(Random.value * attacker_power) + attacker_power;
            int defender_roll = (int)(Random.value * defender_power) + defender_power;

            float result = ((float)(attacker_roll)) / ((float)(defender_roll));

            float critical_win = 0.25f;
            float good_win     = 0.50f;
            float close_win    = 0.75f;
            float costly_win   = 1.0f;

            if (result <= critical_win) // attacker / defender <= 0.25 = defender critial win
            {
                attacker_ship.TakeDamage(100);
            }
            else if (result <= good_win) // attacker / defender <= 0.50 = defender good win
            {
                attacker_ship.TakeDamage(75 + (int)(25 * Random.value));
            }
            else if (result <= close_win) // attacker / defender <= 0.75 = defender close win
            {
                attacker_ship.TakeDamage(50 + (int)(50 * Random.value));
            }
            else if (result <= costly_win) // attacker / defender <= 1.00 = defender costly win
            {
                attacker_ship.TakeDamage(25 + (int)(75 * Random.value));
                defender_ship.TakeDamage((int)(25 * Random.value));
            }
            else if (result <= 1 + (1 - close_win)) // attacker / defender <= 1.25 = attacker costly win
            {
                attacker_ship.TakeDamage((int)(25 * Random.value));
                defender_ship.TakeDamage(25 + (int)(75 * Random.value));
            }
            else if (result <= 1 + (1 - good_win)) // attacker / defender <= 1.50 = attacker close win
            {
                defender_ship.TakeDamage(50 + (int)(50 * Random.value));
            }
            else if (result <= 1 + (1 - critical_win)) // attacker / defender <= 1.75 = attacker good win
            {
                defender_ship.TakeDamage(75 + (int)(25 * Random.value));
            }
            else  // attacker / defender > 1.75 = attacker critical win
            {
                defender_ship.TakeDamage(100);
            }
        }
    }
}
