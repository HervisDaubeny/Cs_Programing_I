using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTree {
    public class Program {

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static FileStream GetFileStream(string file) {

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);

            return fileStream;
        }
    }
}
