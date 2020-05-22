using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using StreetsLibrary;

namespace Task2
{
    public class Task2
    {
        public static void Main()
        {
            var streets = Deserialize($"../../../Var2/bin/debug/out.ser");

            var w7str = (from street in streets
                         where ~street % 2 != 0
                         where !street
                         select street).ToArray();

            if (w7str.Count() == 0)
                Console.WriteLine("Улицы не найдены");
            else
                foreach (var str in w7str)
                    Console.WriteLine(str);
        }

        private static List<Street> Deserialize(string path)
        {

            XmlSerializer xml_form = new XmlSerializer(typeof(List<Street>));
            var streets = new List<Street>();
            try
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    streets = (List<Street>)xml_form.Deserialize(stream);
                    Console.WriteLine("Deserializaation is done");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Неопознанная ошибка");
            }

            return streets;
        }
    }
}
