using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Classes.Helper
{
    public class Person
    {
        public string FirstName = "";
        public string LastName = "";
        public string MiddleName = "";
        public string Honorific = "";
        public string Title = "";
        public string Rank = "";

        public Person()
        {
        }

        public override string ToString()
        {
            string result = "";
            if (Rank != "")
            {
                result += Rank + ", ";
            }
            if (Honorific != "")
            {
                if (Honorific.EndsWith("."))
                {
                    result += Honorific + " ";
                }
                else
                {
                    result += Honorific + " ";
                }
            }
            if (FirstName != "")
            {
                result += FirstName + " ";
            }
            if (MiddleName != "")
            {
                result += MiddleName + " ";
            }
            if (LastName != "")
            {
                result += LastName + " ";
            }
            return result;
        }
    }

    public class Tuple<T1, T2>
    {
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }
        internal Tuple(T1 first, T2 second)
        {
            Item1 = first;
            Item2 = second;
        }

        public override string ToString()
        {
            return Item1.ToString() + " " + Item2.ToString();
        }
    }

    class NameGenerator : MonoBehaviour
    {
        public TextAsset basic_syl_processed = null;

        private static NameGenerator _main = null;
        private List<Tuple<int, string>> basic_syllables = null;
        private List<Tuple<int, string>> honor = null;
        private List<Tuple<int, string>> ranks = null;
        private int total_basic = 0;
        private int total_ranks = 0;
        private int total_honor = 0;

        public void InitializeNameGenerator()
        {
            if (_main == null)
            {
                _main = this;
            }
        }

        public void Awake()
        {
            InitializeNameGenerator();
            //GenSylFile();
            Load("syl", out basic_syllables, out total_basic);
            Load("ranks", out ranks, out total_ranks);
            Load("honor", out honor, out total_honor);
        }

        public Tuple<string, string> RandomFirstLastName()
        {
            string First = RandomName(1, 4, 2, 13);
            string Last = RandomName(2, 5, 2, 13);
            while (First == Last)
            {
                Last = RandomName();
            }
            return new Tuple<string, string>(First, Last);
        }

        public bool BeginsInVowel(string f)
        {
            string[] vowels = { "a", "e", "i", "o", "u", "y" };
            foreach (var vowel in vowels)
            {
                if (f.StartsWith(vowel))
                {
                    return true;
                }
            }
            return false;
        }

        public bool EndsInVowel(string f)
        {
            string[] vowels = { "a", "e", "i", "o", "u", "y" };
            foreach (var vowel in vowels)
            {
                if (f.EndsWith(vowel))
                {
                    return true;
                }
            }
            return false;
        }

        public Person RandomPerson()
        {
            Person p = new Person();
            bool middleName = Random.value > 0.8f;
            bool lastName = Random.value > 0.125f;
            bool rank = middleName ? Random.value > 0.8f : Random.value > 0.4f;
            bool honorific = !rank && Random.value > 0.5f;

            p.FirstName = RandomName(1, 4, 2, 13);
            if (lastName)
            {
                p.LastName = RandomName(2, 5, 2, 13);
                if (middleName)
                {
                    p.MiddleName = RandomName(1, 3, 1, 5);
                }
            }
            if (rank)
            {
                p.Rank = RandomRank();
            }
            if (honorific)
            {
                p.Honorific = RandomHonorific();
            }
            return p;
        }

        public string RandomHonorific()
        {
            int weighted = Random.Range(0, total_honor);
            int count = 0;
            foreach (var x in honor)
            {
                count += x.Item1;
                if (count >= weighted)
                {
                    return x.Item2;
                }
            }
            return ranks[0].Item2;
        }

        public string RandomRank()
        {
            int weighted = Random.Range(0, total_ranks);
            int count = 0;
            foreach (var x in ranks)
            {
                count += x.Item1;
                if (count >= weighted)
                {
                    return x.Item2;
                }
            }
            return ranks[0].Item2;
        }

        public string RandomName(int minSyllables = 1, int maxSyllables = 4, int minChar = 0, int maxChar = 0)
        {
            int num = maxSyllables;
            if (minSyllables < 1)
            {
                minSyllables = 1;
            }
            if (minChar == 0)
            {
                minChar = (minSyllables - 1) * 3 + 2;
            }
            if (maxChar == 0)
            {
                maxChar = maxSyllables * 4;
            }
            int max_char_length = Random.Range(minChar, maxChar);
            int max_to_add = Random.Range(minSyllables, maxSyllables);

            List<string> syllables = new List<string>();
            for (int i = 0; i < num; i++)
            {
                int weighted = Random.Range(0, total_basic);
                int count = 0;
                foreach (var x in basic_syllables)
                {
                    count += x.Item1;
                    if (count > weighted && (syllables.Count < 2 || syllables[syllables.Count - 1] != x.Item2))
                    {
                        syllables.Add(x.Item2);
                        break;
                    }
                }
            }
            string result = syllables[0];
            int char_length = result.Length;
            int added = 1;
            for (int i = 1; i < syllables.Count; i++)
            {
                string cur = syllables[i];
                if (EndsInVowel(result) && !BeginsInVowel(cur))
                {
                    result += cur;
                    char_length += cur.Length;
                    added++;
                }
                else if (!EndsInVowel(result) && BeginsInVowel(cur))
                {
                    result += cur;
                    char_length += cur.Length;
                    added++;
                }
                else if (result[result.Length - 1] == cur[0] && cur[0] != 'i')
                {
                    result += cur;
                    char_length += cur.Length;
                    added++;
                }
                if (added >= max_to_add || char_length >= max_char_length)
                {
                    break;
                }
            }

            return Capitalize(result);
        }

        public string Capitalize(string s)
        {
            if (s.Length == 0)
            {
                return "";
            }
            string first = s[0].ToString().ToUpper();
            if (s.Length == 1)
            {
                return first;
            }
            return first + s.Substring(1);
        }

        private void Load(string filename, out List<Tuple<int, string>> output, out int totalFreq)
        {
            output = new List<Tuple<int, string>>();
            totalFreq = 0;
            TextAsset text = Resources.Load(Path.Combine("NameGeneration", filename)) as TextAsset;
            string content = text.text;
            if (content == "")
            {
                content = System.Text.Encoding.Default.GetString(text.bytes);
            }
            string[] s1 = content.Split('\n');
            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i].Length >= 3)
                {

                    string[] line = s1[i].Split(',');
                    if (line.Length == 2)
                    {
                        int freq;
                        if (!int.TryParse(line[1], out freq))
                        {
                            freq = 1;
                            Debug.Log("FAILED: " + s1[i]);
                        }
                        totalFreq += freq;
                        output.Add(new Tuple<int, string>(freq, line[0]));
                    }
                }
            }
        }

        private void GenSylFile()
        {
            Dictionary<string, int> syl_basic = new Dictionary<string, int>();
            TextAsset text = Resources.Load("NameGeneration/syllables") as TextAsset;
            string content = text.text;
            if (content == "")
            {
                content = System.Text.Encoding.Default.GetString(text.bytes);
            }
            string[] s1 = content.Split('\n');
            foreach (string line in s1)
            {
                string[] syls = splitBasicSylLine(line);
                foreach (string s in syls)
                {
                    string x = Regex.Replace(s, @"[^a-z]+", string.Empty);
                    if (!syl_basic.ContainsKey(x))
                    {
                        syl_basic.Add(x, 1);
                    }
                    else
                    {
                        syl_basic[x]++;
                    }
                }
            }

            List<Tuple<int, string>> sortedList = new List<Tuple<int, string>>();
            int total = 0;
            foreach (string x in syl_basic.Keys)
            {
                sortedList.Add(new Tuple<int, string>(syl_basic[x], x));
                total += syl_basic[x];
            }

            sortedList = sortedList.OrderBy(x => -x.Item1).ToList();
            using (StreamWriter s = new StreamWriter("syl.txt", false))
            {
                foreach (var val in sortedList)
                {
                    string x = val.Item2;
                    int y = val.Item1;
                    s.WriteLine(x + "," + y);
                }
                s.WriteLine("TOTAL," + total);
            }
        }

        private string[] splitBasicSylLine(string line)
        {
            string[] s1 = line.Split('=');
            if (s1.Length >= 2)
            {
                string[] syls = s1[1].Split('·');
                return syls;
            }
            return new string[0];
        }

        public static NameGenerator Main
        {
            get { return _main; }
        }
    }
}
