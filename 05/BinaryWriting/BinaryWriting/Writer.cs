using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryWriting {
    class Writer {

        public BinaryWriter MyWriter { get; set; }

        public Writer(Stream outputFile) {

            this.MyWriter = new BinaryWriter(outputFile);
        }

        public void WriteByte(uint myByte) {

            MyWriter.Write(myByte);
            MyWriter.Flush();
        }

        public void Dispose() {

            this.MyWriter = null;
        }
    }
}
