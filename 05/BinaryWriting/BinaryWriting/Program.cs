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

            uint myBinaryNumber = 16;
            myBinaryNumber <<= 1;

            //1st bit is 0
            //2nd - 24th bits are lowest 23 bits of weight of vertex
            //25th - 32nd bits are value of chracter in ascii
            //Let's make: 0100_0000_0000_0000_0000_0001_0110_0001 ~ 0x40 0x00 0x01 0x61
            //Wich equals 6101 0040 in little endian (PSPAD)

            uint leaf = 0;
            uint weight = 0b_0111_1111_0100_0000_0000_0000_0000_0001;
            uint charValue = 97;

            //shift weight
            weight <<= 9;
            weight >>= 1;

            uint result = leaf | weight | charValue;

            Console.WriteLine("Output from Console.WriteLine():");
            Console.WriteLine(result);

            Console.WriteLine("Output from BinaryWriter.Write()");
            writer.WriteByte(result);

            writer.Dispose();

            Console.ReadKey();
        }
    }
}
