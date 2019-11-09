using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTree {

    public class ArgumentCheck {

        /// <summary>
        /// Checks the number of arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>True if there is precisely one argument, false otherwise.</returns>
        public static bool IsOneArgument( string[] args ) {


            if (args.Length != 1) {

                return false;
            }

            return true;
        }

        /// <summary>
        /// Tries to open file for reading, if succesfull, it also closes and disposes of thusly created object.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>True on succes, false if any exception is encountered.</returns>
        public static bool IsFileReadable( string path ) {

            FileStream testStream;

            try {

                testStream = File.OpenRead(path);
            }
            catch (Exception e) {

                return false;
            }

            testStream.Close();
            testStream.Dispose();

            return true;
        }

        /// <summary>
        /// Tries to open file for writing, if succesfull, it also closes and disposes of thusly created object.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>True on success, false if any exception is encountered.</returns>
        public static bool IsFileWritable( string path ) {

            FileStream testStream;

            try {

                testStream = File.Create(path);
            }
            catch (Exception e) {

                return false;
            }

            testStream.Close();
            testStream.Dispose();

            return true;
        }
    }
}
