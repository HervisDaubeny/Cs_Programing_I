using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTreeII {
    public class SupremeCommander {
        private Reader Reader { get; set; }
        private Forest Forest { get; set; }
        private Coder Coder { get; set; }
        private FileStream FileStream { get; set; }
        public ulong[] Occurance { get; private set; }

        public static SupremeCommander GetSupremeCommander( string inputPath, string outputPath ) {
            SupremeCommander commander = null;
            try {
                commander = new SupremeCommander(inputPath, outputPath);
            }
            catch (Exception e) {
                return null;
            }
            return commander;
        }

        /// <summary>
        /// Creates new SupremeCommander object.
        /// </summary>
        /// <param name="file">FileStream of the input file.</param>
        private SupremeCommander( string inputPath, string outputPath ) {
            FileStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
            Reader = new Reader(FileStream);
            Forest = new Forest();
            Writer.SetBinaryWriter(outputPath);
            Occurance = new ulong[ 256 ];
        }
        public void CreateHuffmanTree() {
            LoadCharacters();
            MakeTreesFromOccurances();
            MakeHuffmanTreeFromTrees();
        }

        /// <summary>
        /// Calls static object Writer to print Huffman tree.
        /// </summary>
        public void PrintHuffmanTree() {
            Writer.WriteTreeToConsole(ExtractHuffmanTree());
        }

        /// <summary>
        /// 
        /// </summary>
        public void PrintCodedHuffmanTreeToFile() {
            Writer.WriteCodedTreeToFile(ExtractHuffmanTree());
        }

        public void PrintCodedFileToFile() {
            SeekStartOfFile();
            InitiateCoder();
            PrintCodedFile();
        }

        /// <summary>
        /// Disposes fo this object properties.
        /// </summary>
        public void Dispose() {
            Reader.Dispose();
            Forest.Dispose();
            Coder.Dispose();
        }

        /// <summary>
        /// Reads characers byte by byte until EOF is reached.
        /// </summary>
        private void LoadCharacters() {
            const int defaultbuffersize = 4096;
            byte[] anotherhappybuffer = default(byte[]);
            while (true) {
                anotherhappybuffer = Reader.ReadBuffer(defaultbuffersize);
                LoadBytesToOccurances(anotherhappybuffer);
                if (anotherhappybuffer.Length < defaultbuffersize) {
                    break;
                }
            }
            anotherhappybuffer = null;
        }

        private void LoadBytesToOccurances(byte[] byteArray) {
            for (int i = 0; i < byteArray.Length; i++) {
                Occurance[ byteArray[ i ] ]++;
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
        private Tree ExtractHuffmanTree() {
            return this.Forest.ExtractCopyOfSmallestTree();
        }

        private void SeekStartOfFile() {
            FileStream.Seek(0, SeekOrigin.Begin);
        }

        private void InitiateCoder() {
            Coder = new Coder(FileStream);
            Coder.BuildCodingTable(ExtractHuffmanTree());
        }

        private void PrintCodedFile() {
            Coder.WriteCodedInputFile();
        }

        /// <summary>
        /// Calls private methods that load data from input, make trees from them and finaly make Huffman tree from trees.
        /// </summary>
    }
}
