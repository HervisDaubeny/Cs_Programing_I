using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTreeII {
    public class Coder {
        private const int DefaultBufferSize = 4096;
        private Byte[][] RootLeafPaths { get; set; }
        private ulong CodedBuffer { get; set; }
        private byte CodedBufferIndex { get; set; }
        private bool FileEnd { get; set; }
        private Reader Reader { get; set; }
        private byte[] Buffer { get; set; }

        public Coder(FileStream file) {
            Reader = new Reader(file);
            RootLeafPaths = new Byte[ 256 ][];
        }

        public void BuildCodingTable( Tree huffmanTree ) {
            SearchForLeaf(huffmanTree, new List<byte>(), 0);
        }

        public void WriteCodedInputFile() {
            GetBuffer();
            SetFileEndFlag();
            CodeBuffer();
        }

        public void Dispose() {
            RootLeafPaths = null;
            Reader.Dispose();
            Buffer = null;
        }

        private void CodeBuffer() {
            bool lastInFile = false;

            for (int i = 0; i < Buffer.Length; i++) {
                if (i ==  Buffer.Length - 1) {
                    if (!FileEnd) {
                        CodeByte(Buffer[ i ], lastInFile);
                        GetBuffer();
                        SetFileEndFlag();
                        i = 0;
                    }
                    else {
                        lastInFile = true;
                    }
                }
                CodeByte(Buffer[ i ], lastInFile);
            }
        }

        public void CodeByte(byte toCode, bool lastByte) {
            int pathBitLength = RootLeafPaths[ toCode ].Length;
            const ulong topOne = 0x8000_0000_0000_0000;

            for (int i = 0; i < pathBitLength; i++) {
                ClearCodedBufferIfFull();
                if (RootLeafPaths[ toCode ][ i ] > 0) {
                    CodedBuffer >>= 1;
                    CodedBuffer |= topOne;
                    CodedBufferIndex++;
                }
                else {
                    CodedBuffer >>= 1;
                    CodedBufferIndex++;
                }
            }
            if (lastByte) {
                int padding = 64 - CodedBufferIndex;
                CodedBuffer >>= padding;
                ClearCodedBufferOnEnd();
            }
        }

        private void AddRecord( byte value, byte[] path ) {
            RootLeafPaths[ value ] = path;
        }

        private void SearchForLeaf( Tree tree, List<Byte> path, int depth ) {
            if (tree.Type != Tree.TreeType.Leaf) {
                path.Add(0);
                SearchForLeaf(tree.LeftSon, path, depth + 1);
                path.RemoveAt(depth);
                path.Add(1);
                SearchForLeaf(tree.RightSon, path, depth + 1);
                path.RemoveAt(depth);
            }
            else {
                AddRecord(tree.Value, path.ToArray());
            }
        }

        private void GetBuffer() {
            Buffer = null;
            Buffer = Reader.ReadBuffer(DefaultBufferSize);
        }

        private void SetFileEndFlag() {
            if (Buffer.Length < DefaultBufferSize) {
                FileEnd = true;
            }
        }

        private void ClearCodedBufferIfFull() {
            if (CodedBufferIndex > 63) {
                Writer.WriteUlongToFile(CodedBuffer);
                CodedBuffer = 0;
                CodedBufferIndex = 0;
            }
        }

        private void ClearCodedBufferOnEnd() {
            byte[] bytes = BitConverter.GetBytes(CodedBuffer);
            int validBytes = CodedBufferIndex / 8;
            if (CodedBufferIndex % 8 > 0)
                validBytes++;
            for (int i = 0; i < validBytes; i++) {
                Writer.WriteByteToFile(bytes[ i ]);
            }
            CodedBuffer = 0;
            CodedBufferIndex = 0;
        }
    }
}
