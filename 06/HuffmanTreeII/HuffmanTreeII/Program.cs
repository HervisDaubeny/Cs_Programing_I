using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTreeII {
    public class Program {
        /// <summary>
        /// Method that is called on start of the program.
        /// </summary>
        /// <param name="args"></param>
        static void Main( string[] args ) {
            string inputPath = default(string);
            string outputPath = default(string);
            SupremeCommander Thor;

            if (!CheckArguments(args)) {
                return;
            }
            inputPath = args[ 0 ];
            outputPath = inputPath + ".huff";
            Thor = SupremeCommander.GetSupremeCommander(inputPath, outputPath);
            if (Thor == null) {
                Console.WriteLine("File Error");
                return;
            }
            Thor.CreateHuffmanTree();
            Thor.PrintCodedHuffmanTreeToFile();
            Thor.PrintCodedFileToFile();
            Thor.Dispose();
        }

        /// <summary>
        /// Goes through arguments and checks if there is correct number of them and if the files are readable (writable).
        /// </summary>
        /// <param name="args">Array of arguments.</param>
        /// <returns>True if the arguments are as expected, false otherwise.</returns>
        public static bool CheckArguments( string[] args ) {
            string error = default(string);
            string inputFile = default(string);
            string outputFile = default(string);

            if (!ArgumentCheck.IsOneArgument(args, out error)) {
                Writer.WriteToConsole(error);
                return false;
            }
            /*
            inputFile = args[ 0 ];
            if (!ArgumentCheck.IsFileReadable(inputFile, out error)) {
                Writer.WriteToConsole(error);
                return false;
            }
            outputFile = inputFile + ".huff";
            if (!ArgumentCheck.IsFileWritable(outputFile, out error)) {
                Writer.WriteToConsole(error);
                return false;
            }
            */
            return true;
        }
    }
}
