using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hervis.Excell;


namespace Excel {
    public class Program {
        static void Main( string[] args ) {
            TextReader reader = default(TextReader);
            TextWriter writer = default(TextWriter);
            SupremeCommander Thor = default(SupremeCommander);

            if (args.Length != 2) {
                Console.WriteLine("Argument Error");
                return;
            }
            try {
                reader = File.OpenText(args[0]);
                writer = File.CreateText(args[ 1 ]);
            }
            catch (Exception) {
                Console.WriteLine("File Error");
                return;
            }

            Thor = SupremeCommander.Initialize(reader, writer);
            Thor.SolveEquations();
            Thor.WriteFile();
            Thor.Dispose();
        }
    }
}
