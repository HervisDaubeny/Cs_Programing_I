using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HuffmanTreeII {
    public class Writer {
        private static BinaryWriter BinaryWriter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public static void SetBinaryWriter( string path ) {
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter = new BinaryWriter(stream);
        }

        /// <summary>
        /// Writes line to console.
        /// </summary>
        /// <param name="text">Text to be printed to console.</param>
        public static void WriteToConsole( string text ) {
            Console.WriteLine(text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public static void WriteUlongToFile( ulong output ) {
            BinaryWriter.Write(output);
            BinaryWriter.Flush();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public static void WriteByteToFile( byte output ) {
            BinaryWriter.Write(output);
            BinaryWriter.Flush();
        }

        /// <summary>
        /// Calls recursive method to build string and then print its output.
        /// </summary>
        /// <param name="tree">Tree object to be printed.</param>
        public static void WriteTreeToConsole( Tree tree ) {
            Console.Write(ConvertHuffmanTreeToString(tree, true));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
        public static void WriteCodedTreeToFile( Tree tree ) {
            WriteBeginTreeToFile();
            CodeAndPrintHuffmanTreeToFile(tree);
            WriteEndTreeToFile();
            BinaryWriter.Flush();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CloseFile() {
            BinaryWriter.Close();
            BinaryWriter.Dispose();
        }

        /// <summary>
        /// Recursively goes through the tree and appends value of each vertex to output string.
        /// </summary>
        /// <param name="tree">Tree object to write.</param>
        /// <returns>String representation of tree given as argument.</returns>
        private static string ConvertHuffmanTreeToString( Tree tree, bool inRoot ) {
            string printableTree = default(string);
            if (!inRoot) {
                printableTree += " ";
            }
            if (tree.Type == Tree.TreeType.Leaf) {
                printableTree += tree.convertLeafValueToString(tree.Value, tree.Weight);
            }
            else {
                printableTree += tree.convertRootValueToString(tree.LeftSon.Weight, tree.RightSon.Weight);
            }
            if (tree.Type != Tree.TreeType.Leaf) {
                printableTree += ConvertHuffmanTreeToString(tree.LeftSon, false);
                printableTree += ConvertHuffmanTreeToString(tree.RightSon, false);
            }
            return printableTree;
        }

        /// <summary>
        /// 
        /// </summary>
        private static void WriteBeginTreeToFile() {
            const ulong head = 0x66_66_7D_6D_7C_75_68_7B;
            BinaryWriter.Write(head);
        }

        /// <summary>
        /// 
        /// </summary>
        private static void WriteEndTreeToFile() {
            const ulong end = 0;
            BinaryWriter.Write(end);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
        private static void CodeAndPrintHuffmanTreeToFile( Tree tree ) {
            ulong codedVertex = tree.ConvertVertexTo64Bits();
            BinaryWriter.Write(codedVertex);
            if (tree.Type != Tree.TreeType.Leaf) {
                CodeAndPrintHuffmanTreeToFile(tree.LeftSon);
                CodeAndPrintHuffmanTreeToFile(tree.RightSon);
            }
        }
    }
}
