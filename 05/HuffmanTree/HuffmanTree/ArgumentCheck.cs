using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTree {
    public class ArgumentCheck {
        private const string ArgumentError = "Argument Error";
        private const string FileError = "File Error";

        /// <summary>
        /// Checks the number of arguments.
        /// </summary>
        /// <param name="args">Array of Program arguments.</param>
        /// <returns>True if there is precisely one argument, false otherwise.</returns>
        public static bool IsOneArgument( string[] args , out string error) {
            error = default(string);
            if (args.Length != 1) {
                error = ArgumentError;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Tries to open file for reading, if succesfull, it also closes and disposes of thusly created object.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>True on succes, false if any exception is encountered.</returns>
        public static bool IsFileReadable( string path, out string error ) {
            FileStream testStream;
            error = default(string);
            try {
                testStream = File.OpenRead(path);
            }
            catch (Exception) {
                error = FileError;
                return false;
            }
            testStream.Close();
            testStream.Dispose();
            return true;
        }
    }
}
