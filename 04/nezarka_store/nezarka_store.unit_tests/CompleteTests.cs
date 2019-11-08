using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nezarka_store;

namespace nezarka_store.unit_tests {

    [TestClass]
    public class CompleteTests {

        [TestMethod]
        public void RunAll_CorrectInput_CreatesFile() {

            StreamWriter wrt = new StreamWriter(new FileStream("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CompleteTests\\NezarkaTest.out", FileMode.OpenOrCreate, FileAccess.ReadWrite));
            StoreViewer storeViewer = new StoreViewer(wrt);
            SupremeCommander Thor = new SupremeCommander();
            bool wrongStoreInput;
            TextReader myReader = File.OpenText("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CompleteTests\\NezarkaTest.in");
            Thor.GetData(myReader, out wrongStoreInput);

            if (wrongStoreInput) {

                Thor.PrintDataError();

                return;
            }

            Thor.InititateCommandProcessing(myReader, storeViewer);

            wrt.Close();
            bool identical = Utils.FileDiff("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CompleteTests\\NezarkaTest.out", "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CompleteTests\\NezarkaExpected.out");

            Assert.IsTrue(identical);
        }
        
        [TestMethod]
        public void RunAll_EmptyFile_ReturnsError() {

            StreamWriter wrt = new StreamWriter(new FileStream("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CompleteTests\\EmptyFileTest.out", FileMode.OpenOrCreate, FileAccess.ReadWrite));
            StoreViewer storeViewer = new StoreViewer(wrt);
            SupremeCommander Thor = new SupremeCommander();
            bool wrongStoreInput;
            TextReader myReader = File.OpenText("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CompleteTests\\EmptyFileTest.in");
            Thor.GetData(myReader, out wrongStoreInput);

            if (wrongStoreInput) {

                Thor.PrintDataError();

                return;
            }

            Thor.InititateCommandProcessing(myReader, storeViewer);

            wrt.Close();
            bool identical = Utils.FileDiff("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CompleteTests\\EmptyFileTest.out", "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CompleteTests\\EmptyFileExpected.out");

            Assert.IsTrue(identical);
        }
    }
}
