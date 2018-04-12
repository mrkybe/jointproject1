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
}