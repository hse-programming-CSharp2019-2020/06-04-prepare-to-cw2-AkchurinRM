using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using StreetsLibrary;

namespace Task1
{
    public class Task1
    {

        static int CRinput(string message)
        {
            int number = 0;
            Console.Write(message);
            while (!int.TryParse(Console.ReadLine(), out number) || number < 1 || number > 1000)
                Console.WriteLine("Num must be in range [1, 1000]");

            return number;
        }

        static Random rnd = new Random();

        static string RndName()
        {
            int Name_length = rnd.Next(5, 16);
            string Name = $"{(char)rnd.Next('A', 'Z' + 1)}";
            for (int i = 0; i < Name_length; i++)
                Name += (char)rnd.Next('a', 'z' + 1);

            return Name;
        }

        private static void RandomizeStreets(List<Street> streets, int n)
        {
            int[] nums = new int[rnd.Next(1, 11)];

            for (int i = 0; i < nums.Length; i++) nums[i] = rnd.Next(1, 101);

            for (int i = 0; i < n; i++) streets.Add(new Street(RndName(), nums));
        }

        /// <summary>
        /// Основная проверка файла "data.txt" на корректность.
        /// </summary>
        /// <param name="p">path - путь к файлу</param>
        /// <returns></returns>
        static bool isCorrect(string p)
        {
            if (!File.Exists(p)) return false;

            string[] streets = File.ReadAllLines(p);
            if (streets.Length == 0) return false;

            bool flag = true;
            foreach (string street in streets)
            {
                string[] houses = street.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                int num = 0;
                int[] nums = houses.Where(stroke => int.TryParse(stroke, out num)).Select(stroke => int.Parse(stroke)).ToArray();

                if (nums == null) return false; // на всякий случай, хотя это невозможно.

                if ((nums.Length == 0) || (nums.Length != houses.Length - 1))
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        public static void Main()
        {
            int N = CRinput("Введите N: ");

            List<Street> streets = new List<Street>();
            if (isCorrect("data.txt"))
            {
                string[] data = File.ReadAllLines("data.txt");
                foreach (string str in data)
                {
                    try
                    {
                        // Сложный выбор: по условию должен быть только 1 пробел, т.е. если 2 пробела, то нужно выкидывать предупреждение?
                        string[] filtered_str = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        int num = 0; // переменная ради TryParse()
                        int[] nums = filtered_str.Where(stroke => int.TryParse(stroke, out num)).Select(stroke => int.Parse(stroke)).ToArray();
                        streets.Add(new Street(filtered_str[0], nums));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Неизвестная ошибка");
                        streets.Clear();
                        RandomizeStreets(streets, N);
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Неверные данные в файле \"data.txt\"");
                RandomizeStreets(streets, N);
            }

            if (streets.Count < N)
                RandomizeStreets(streets, N - streets.Count);

            foreach (Street street in streets) Console.WriteLine(street);

            Serialize(streets);
        }

        private static void Serialize(List<Street> streets)
        {
            using (FileStream fs = new FileStream("out.ser", FileMode.Create))
            {
                XmlSerializer xml_form = new XmlSerializer(typeof(List<Street>));
                xml_form.Serialize(fs, streets);

                Console.WriteLine("Serialization has been done");
            }
        }
    }
}

