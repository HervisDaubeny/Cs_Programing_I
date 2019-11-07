using System;

namespace OHledaniBugu {
    class Program {
        static void Main( string[] args ) {

            string line = Console.ReadLine();
            if (line == null) {

                Console.WriteLine("Error!");
                return;
            }

            bool parse = int.TryParse(line, out int a);
            if (!parse || a < 0) {

                Console.WriteLine("Error!");
                return;
            }

            line = Console.ReadLine();
            if (line == null) {

                Console.WriteLine("Error!");
                return;
            }

            parse = int.TryParse(line, out int b);
            if (!parse || b < 0) {

                Console.WriteLine("Error!");
                return;
            }

            int result = Math.Abs(a - b);

            Console.WriteLine("Result: {0}", result);
        }
    }
}