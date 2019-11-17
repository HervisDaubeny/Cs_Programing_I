using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HuffmanTree {
    public class Writer {

        private StreamWriter streamWriter { get; set; }

        /// <summary>
        /// Writes line to console.
        /// </summary>
        /// <param name="text">Text to be printed to console.</param>
        public static void WriteToConsole( string text ) {

            Console.WriteLine(text);
        }

        /// <summary>
        /// Calls recursive method to build string and then print its output.
        /// </summary>
        /// <param name="tree">Tree object to be printed.</param>
        public static void WriteTreeToConsole( Tree tree ) {

            Console.Write(ConvertTreeToString(tree, true));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        private static string ConvertTreeToString( Tree tree, bool inRoot ) {

            string printableTree = default(string);

            if (!inRoot) {
                printableTree += " " + tree.Value;
            }
            else {
                printableTree += tree.Value;
            }

            if (tree.Type != Tree.TreeType.Leaf) {

                printableTree += ConvertTreeToString(tree.LeftSon, false);
                printableTree += ConvertTreeToString(tree.RightSon, false);
            }

            return printableTree;
        }
    }
}
