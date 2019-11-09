using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTree {
    public class Program {

        static void Main( string[] args ) {

            Run(args);
        }

        public static void Run(string[] args) {

            string error = default(string);

            if(!ArgumentCheck.IsOneArgument(args, out error)) {

                Writer.WriteToConsole(error);

                return;
            }

            if(!ArgumentCheck.IsFileReadable(args[0], out error)) {

                Writer.WriteToConsole(error);

                return;
            }
        }
    }
}
