using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryWriting {
    class Program {
        static void Main( string[] args ) {

            Writer writer = new Writer(new FileStream(args[0], FileMode.OpenOrCreate, FileAccess.Write));

            uint myBinaryNumber = 0b1000_0001;

            Console.WriteLine("Output from Console.WriteLine():");
            Console.WriteLine(myBinaryNumber);

            Console.WriteLine("Output from BinaryWriter.Write()");
            writer.WriteByte(myBinaryNumber);

            writer.Dispose();

            Console.ReadKey();
        }
    }
}
