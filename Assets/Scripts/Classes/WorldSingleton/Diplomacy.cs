using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Classes.Mobile;
using UnityEngine;

namespace Assets.Scripts.Classes.WorldSingleton
{
    public partial class Overseer : Static.Static
    {
        [SerializeField]
        public static List<string> FactionNamesListMain;

        [SerializeField]
        public static List<string> FactionNamesListOther;

        [SerializeField]
        public static List<Faction> Factions;

        [SerializeField]
        public static List<Faction> MainFactions;

        [SerializeField]
        public static List<Faction> OtherFactions;

        [SerializeField]
        public static List<FactionLink> Links;

        private void InitializeFactions()
        {
            FactionNamesListMain = new List<string>
            {
                "Communist",
                "Libertarian",
                "Freedom",
                "Duty",
                "Independent"
            };
            FactionNamesListOther = new List<string>
            {
                "Robots",
                "Pirates",
                "Player"
            };
            Factions = new List<Faction>();
            MainFactions = new List<Faction>();
            OtherFactions = new List<Faction>();
            Links = new List<FactionLink>();
        }

        private Faction GetRandomMainFaction()
        {
            int numFactions = MainFactions.Count;
            int val = Random.Range(0, numFactions);
            return MainFactions[val];
        }

        private void CreateFactions()
        {
            InitializeFactions();
            int count = 0;
            var FactionNamesList = new List<string>();
            FactionNamesList.AddRange(FactionNamesListMain);
            FactionNamesList.AddRange(FactionNamesListOther);
            foreach (var name in FactionNamesList)
            {
                Faction f = new Faction(name);
                float frac = count / (FactionNamesListMain.Count * 2f);
                f.ColorPrimary = Color.HSVToRGB(frac, 0.8f, 1f);
                f.ColorSecondary = Color.HSVToRGB(frac + 0.5f, 0.8f, 1f);
                Factions.Add(f);
                if (FactionNamesListMain.Contains(name))
                {
                    MainFactions.Add(f);
                }
                else
                {
                    OtherFactions.Add(f);
                }
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
            f.SelfHostile = true;
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


        public enum BattleResult { ATTACK_WIN, TIE, DEFEND_WIN}
        /// <summary>
        /// Makes two ships fight and returns who did the best.
        /// </summary>
        /// <param name="attacker_ship"></param>
        /// <param name="defender_ship"></param>
        /// <returns></returns>
        public BattleResult ResolveShipCombat(Spaceship attacker_ship, Spaceship defender_ship)
        {
            IEnumerable<Spaceship> ship1_allies = attacker_ship.GetInInteractionRange<Spaceship>().Where(x => x.Pilot.Faction == attacker_ship.Pilot.Faction);
            IEnumerable<Spaceship> ship2_allies = defender_ship.GetInInteractionRange<Spaceship>().Where(x => x.Pilot.Faction == defender_ship.Pilot.Faction);

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
                attacker_ship.TakeDamage(100, defender_ship);
                return BattleResult.DEFEND_WIN;
            }
            else if (result <= good_win) // attacker / defender <= 0.50 = defender good win
            {
                attacker_ship.TakeDamage(75 + (int)(25 * Random.value), defender_ship);
                return BattleResult.DEFEND_WIN;
            }
            else if (result <= close_win) // attacker / defender <= 0.75 = defender close win
            {
                attacker_ship.TakeDamage(50 + (int)(50 * Random.value), defender_ship);
                return BattleResult.DEFEND_WIN;
            }
            else if (result <= costly_win) // attacker / defender <= 1.00 = defender costly win
            {
                attacker_ship.TakeDamage(25 + (int)(75 * Random.value), defender_ship);
                defender_ship.TakeDamage((int)(25 * Random.value), attacker_ship);
                return BattleResult.TIE;
            }
            else if (result <= 1 + (1 - close_win)) // attacker / defender <= 1.25 = attacker costly win
            {
                attacker_ship.TakeDamage((int)(25 * Random.value), defender_ship);
                defender_ship.TakeDamage(25 + (int)(75 * Random.value), attacker_ship);
                return BattleResult.TIE;
            }
            else if (result <= 1 + (1 - good_win)) // attacker / defender <= 1.50 = attacker close win
            {
                defender_ship.TakeDamage(50 + (int)(50 * Random.value), attacker_ship);
                return BattleResult.ATTACK_WIN;
            }
            else if (result <= 1 + (1 - critical_win)) // attacker / defender <= 1.75 = attacker good win
            {
                defender_ship.TakeDamage(75 + (int)(25 * Random.value), attacker_ship);
                return BattleResult.ATTACK_WIN;
            }
            else  // attacker / defender > 1.75 = attacker critical win
            {
                defender_ship.TakeDamage(100, attacker_ship);
                return BattleResult.ATTACK_WIN;
            }
        }
    }
}
