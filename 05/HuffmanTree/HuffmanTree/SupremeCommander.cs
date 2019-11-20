using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTree {
    public class SupremeCommander {
        private Reader Reader;
        private Forest Forest;
        public ulong[] Occurance { get; private set; }

        /// <summary>
        /// Creates new SupremeCommander object.
        /// </summary>
        /// <param name="file">FileStream of the input file.</param>
        public SupremeCommander(FileStream file) {
            this.Reader = new Reader(file);
            this.Forest = new Forest();
            Occurance = new ulong[ 256 ];
        }

        /// <summary>
        /// Reads characers byte by byte until EOF is reached.
        /// </summary>
        private void LoadCharacters() {
            byte anotherHappyByte = default(byte);
            while (true) {
                bool result = Reader.ReadByte(out anotherHappyByte);
                if (!result) {
                    break;
                }
                Occurance[ (int) anotherHappyByte ]++;
            }
        }

        /// <summary>
        /// Calls PlantForest method on own instance of Forest, creating Tree representation of INPUT data.
        /// </summary>
        private void MakeTreesFromOccurances() {
            Forest.PlantForest(Occurance);
        }

        /// <summary>
        /// Calls private method, that builds Huffman tree.
        /// </summary>
        private void MakeHuffmanTreeFromTrees() {
            Forest.GrowHuffmansTree();
        }

        /// <summary>
        /// Removes Huffman tree from the forrest.
        /// </summary>
        /// <returns>Huffman tree.</returns>
        private Tree ExtractHuffmanTreeForPrinting() {
            return this.Forest.ExtractSmallestTree();
        }

        /// <summary>
        /// Calls private methods that load data from input, make trees from them and finaly make Huffman tree from trees.
        /// </summary>
        public void CreateHuffmanTree() {
            LoadCharacters();
            MakeTreesFromOccurances();
            MakeHuffmanTreeFromTrees();
        }

        /// <summary>
        /// Calls static object Writer to print Huffman tree.
        /// </summary>
        public void PrintHuffmanTree() {
            Writer.WriteTreeToConsole(ExtractHuffmanTreeForPrinting());
        }

        /// <summary>
        /// Disposes fo this object properties.
        /// </summary>
        public void Dispose() {
            this.Reader.Dispose();
            this.Forest.Dispose();
        }
    }
}
