using System;
using System.Linq;

namespace StreetsLibrary
{
    [Serializable]
    public class Street
    {
        public string name { get; set; }

        public int[] houses { get; set; }

        public Street()
        {
            name = "Lenin's";
            houses = new int[] { 1, 2, 3 };
        }

        public Street(string name, int[] houses)
        {
            this.name = name;
            this.houses = houses;
        }

        public static int operator ~(Street street) => street.houses.Length;

        public static bool operator !(Street street)
        {
            var w7 = from house in street.houses
                     where house.ToString().Contains("7")
                     select house;
            if (w7.Count() >= 1)
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            string result = name;
            foreach (int house in houses)
                result += $" {house}";

            return result;
        }
    }
}

