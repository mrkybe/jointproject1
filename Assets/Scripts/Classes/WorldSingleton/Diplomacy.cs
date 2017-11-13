using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Classes.WorldSingleton
{
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
            FactionNamesList = new List<string>();
            FactionNamesList.Add("Red");
            FactionNamesList.Add("Green");
            FactionNamesList.Add("Blue");
            FactionNamesList.Add("Yellow");
            FactionNamesList.Add("Pirates");
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
            foreach (var name in FactionNamesList)
            {
                Faction f = new Faction(name);
                Factions.Add(f);
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

            //Debug.Log("Created Factions!");

            foreach (var link in Links)
            {
                //Debug.Log(link.ToString());
            }
        }
    }
}
