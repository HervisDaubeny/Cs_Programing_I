using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTree {
    public class Program {
        /// <summary>
        /// Method that is called on start of the program.
        /// </summary>
        /// <param name="args"></param>
        static void Main( string[] args ) {
            SupremeCommander Thor;
            if (!CheckArguments(args)) {
                return;
            }
            Thor = new SupremeCommander(GetFileStream(args[ 0 ]));
            Thor.CreateHuffmanTree();
            Thor.PrintHuffmanTree();
            Thor.Dispose();
        }

        /// <summary>
        /// Goes through arguments and checks if there is correct number of them and if the files are readable (writable).
        /// </summary>
        /// <param name="args">Array of arguments.</param>
        /// <returns>True if the arguments are as expected, false otherwise.</returns>
        public static bool CheckArguments(string[] args) {
            string error = default(string);
            if (!ArgumentCheck.IsOneArgument(args, out error)) {
                Writer.WriteToConsole(error);
                return false;
            }
            if (!ArgumentCheck.IsFileReadable(args[ 0 ], out error)) {
                Writer.WriteToConsole(error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Takes in string containing path to the file and opens file stream.
        /// </summary>
        /// <param name="file">Path to the file.</param>
        /// <returns>FileStream to the file.</returns>
        public static FileStream GetFileStream(string file) {
            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            return fileStream;
        }
    }
}
