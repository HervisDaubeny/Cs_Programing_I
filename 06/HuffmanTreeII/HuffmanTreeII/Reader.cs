using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTreeII {
    public class Reader {
        BinaryReader BinaryReader;

        /// <summary>
        /// Creates an instance of Reader. Takes in FileStream of the file it is supposed to read.
        /// </summary>
        /// <param name="file">FileStream of the file to be read.</param>
        public Reader( FileStream file ) {
            this.BinaryReader = new BinaryReader(file);
        }

        /// <summary>
        /// Tries to read one byte. From file opend in this.binaryReader.
        /// </summary>
        /// <param name="nextByte">Out byte read. If EOF is reached, nextByte has default value.</param>
        /// <returns>True on succes. False if EOF was reached.</returns>
        public bool ReadByte( out byte nextByte) {
            nextByte = default(byte);
            try {
                nextByte = this.BinaryReader.ReadByte();
            }
            catch (Exception exe) {
                if (exe is System.IO.EndOfStreamException) {
                    return false;
                }
                else {
                    throw;
                }
            }
            return true;
        }

        public byte[] ReadBuffer( int bufferSize ) {
            return BinaryReader.ReadBytes(bufferSize);
        }

        /// <summary>
        /// Closes and disposes of this.binaryReader.
        /// </summary>
        public void Dispose() {
            BinaryReader.Close();
            BinaryReader.Dispose();
        }
    }
}
