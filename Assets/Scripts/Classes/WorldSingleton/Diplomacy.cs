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
    }
}
