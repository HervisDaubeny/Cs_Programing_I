using System;
using System.IO;

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
            int count = 0;
            StreamReader Str;

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

                count += splited.Length;
            }

            Console.WriteLine(count);
        }
    }
}
