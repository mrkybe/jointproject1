using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Classes.Static;
using UnityEngine;

namespace Assets.Scripts.Classes.WorldSingleton
{
    /// <summary>
    /// A single Faction.  Tracks what belongs to it, who its at war with.
    /// </summary>
    public class Faction
    {
        public String Name;
        public Color ColorPrimary;
        public Color ColorSecondary;
        public List<FactionLink> MyLinks;
        public bool SelfHostile = false;
        private List<Planet> OwnedPlanets;

        public Faction(String name)
        {
            Name = name;
            MyLinks = new List<FactionLink>();
            OwnedPlanets = new List<Planet>();
            ColorPrimary = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            ColorSecondary = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }

        // adjust relations between myself and named faction by amount
        public bool AddKarma(string name, float amount)
        {
            throw new NotImplementedException();
        }

        // assess total value of my assets
        public void CalculateEconomicStanding()
        {
            throw new NotImplementedException();
        }

        // assess total military strength
        public void CalculateMilitaryStanding()
        {
            throw new NotImplementedException();
        }

        // assess military threat level, look at military strength of enemies compared to own military strength
        public void CalculateDEFCONLevel()
        {
            throw new NotImplementedException();
        }

        public void Unown(Planet planet)
        {
            OwnedPlanets.Remove(planet);
        }

        public void Own(Planet planet)
        {
            if (!OwnedPlanets.Contains(planet))
            {
                OwnedPlanets.Add(planet);
            }
        }

        public bool HostileWith(Faction myFaction)
        {
            if (this == myFaction && SelfHostile)
            {
                return true;
            }
            foreach (FactionLink link in MyLinks)
            {
                if (link.a == myFaction || link.b == myFaction)
                {
                    if (link.Friendlyness < 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public class FactionLink
    {
        public Faction a;
        public Faction b;
        public float Friendlyness = 0f;

        public FactionLink(Faction a_in, Faction b_in)
        {
            a = a_in;
            b = b_in;
            Friendlyness = 0;
        }

        public override string ToString()
        {
            return "(" + a.Name + " - " + b.Name + ")";
        }
    }

    class FactionLinkEqualityComparer : IEqualityComparer<FactionLink>
    {
        public bool Equals(FactionLink x, FactionLink y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == y)
            {
                return true;
            }
            if (x.a == y.a && x.b == y.b)
            {
                return true;
            }
            if (x.b == y.a && x.a == y.b)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(FactionLink obj)
        {
            String name1 = obj.a.Name;
            String name2 = obj.b.Name;
            if (String.CompareOrdinal(name1, name2) < 0)
            {
                return (name1 + name2).GetHashCode();
            }
            else
            {
                return (name2 + name1).GetHashCode();
            }
        }
    }
}
