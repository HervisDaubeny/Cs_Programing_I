using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nezarka_store;

namespace nezarka_store.unit_tests {

    [TestClass]
    public class DataParsingTests {

        [TestMethod]
        public void Parse_NoBeginInput_DataError() {
            StreamWriter wrt = new StreamWriter(new FileStream("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\InputParsingTests\\NoBeginTest.out", FileMode.OpenOrCreate, FileAccess.ReadWrite));
            StoreViewer storeViewer = new StoreViewer(wrt);
            SupremeCommander Thor = new SupremeCommander();
            bool wrongStoreInput;
            TextReader myReader = File.OpenText("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\InputParsingTests\\NoBeginTest.in");
            Thor.GetData(myReader, out wrongStoreInput);

            if (wrongStoreInput) {

                Thor.PrintDataError();

                return;
            }

            Thor.InititateCommandProcessing(myReader, storeViewer);

            wrt.Close();
            bool identical = Utils.FileDiff("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\InputParsingTests\\NoBeginTest.out", "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\InputParsingTests\\NeBeginExpected.out");

            Assert.IsTrue(identical);
        }

        [TestMethod]
        public void Parse_NoEndInput_DataError() {

            StreamWriter wrt = new StreamWriter(new FileStream("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\InputParsingTests\\NoEndTest.out", FileMode.OpenOrCreate, FileAccess.ReadWrite));
            StoreViewer storeViewer = new StoreViewer(wrt);
            SupremeCommander Thor = new SupremeCommander();
            bool wrongStoreInput;
            TextReader myReader = File.OpenText("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\InputParsingTests\\NoEndTest.in");
            Thor.GetData(myReader, out wrongStoreInput);

            if (wrongStoreInput) {

                Thor.PrintDataError();

                return;
            }

            Thor.InititateCommandProcessing(myReader, storeViewer);

            wrt.Close();
            bool identical = Utils.FileDiff("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\InputParsingTests\\NoEndTest.out", "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\InputParsingTests\\NoEndExpected.out");

            Assert.IsTrue(identical);
        }
    }
}
