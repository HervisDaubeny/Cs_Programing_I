using System;
using System.IO;
using System.Collections.Generic;

namespace count_words
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 1)
            {
                Console.WriteLine("Argument Error");

                return;
            }

            string source = args[0];

            StreamReader Str;
            SortedDictionary<string, int> words = new SortedDictionary<string, int>();

            try
            {
                Str = new StreamReader(source);
            }
            catch (Exception e)
            {
                Console.WriteLine("File Error");

                return;
            }

            string line = null;
            char[] field = { ' ', '\n', '\t' };

            while ((line = Str.ReadLine()) != null)
            {
                string[] splited = line.Split(field, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < splited.Length; i++)
                {

                    if (words.ContainsKey(splited[i]))
                    {
                        words[splited[i]]++;
                    }
                    else
                    {
                        words.Add(splited[i], 1);
                    }
                }
            }

            foreach (var item in words)
            {
                Console.WriteLine("{0}: {1}", item.Key, item.Value);
            }
        }
    }
}