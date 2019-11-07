using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nezarka_store;

namespace nezarka_store.unit_tests {

    [TestClass]
    public class CompleteTest {

        [TestMethod]
        public void RunAll_CorrectInput_CreatesFile() {

            #region Correct input
            {
                StreamWriter wrt = new StreamWriter("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\my_NezarkaTest.out");
                StoreViewer storeViewer = new StoreViewer(wrt);
                SupremeCommander Thor = new SupremeCommander();
                bool wrongStoreInput;
                TextReader myReader = File.OpenText("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\NezarkaTest.in");
                Thor.GetData(myReader, out wrongStoreInput);

                if (wrongStoreInput) {

                    Thor.PrintDataError();

                    return;
                }

                Thor.InititateCommandProcessing(myReader, storeViewer);

                wrt.Close();
                bool identical = Utils.FileDiff("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\my_NezarkaTest.out", "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\NezarkaTest.out");

                Assert.IsTrue(identical);
            }
            #endregion

            #region Correct input
            {
                StreamWriter wrt = new StreamWriter("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\my_NezarkaTest.out");
                StoreViewer storeViewer = new StoreViewer(wrt);
                SupremeCommander Thor = new SupremeCommander();
                bool wrongStoreInput;
                TextReader myReader = File.OpenText("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\NezarkaTest.in");
                Thor.GetData(myReader, out wrongStoreInput);

                if (wrongStoreInput) {

                    Thor.PrintDataError();

                    return;
                }

                Thor.InititateCommandProcessing(myReader, storeViewer);

                wrt.Close();
                bool identical = Utils.FileDiff("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\my_NezarkaTest.out", "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\NezarkaTest.out");

                Assert.IsTrue(identical);
            }
            #endregion

            #region Correct input
            {
                StreamWriter wrt = new StreamWriter("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\my_NezarkaTest.out");
                StoreViewer storeViewer = new StoreViewer(wrt);
                SupremeCommander Thor = new SupremeCommander();
                bool wrongStoreInput;
                TextReader myReader = File.OpenText("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\NezarkaTest.in");
                Thor.GetData(myReader, out wrongStoreInput);

                if (wrongStoreInput) {

                    Thor.PrintDataError();

                    return;
                }

                Thor.InititateCommandProcessing(myReader, storeViewer);

                wrt.Close();
                bool identical = Utils.FileDiff("D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\my_NezarkaTest.out", "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\NezarkaTest.out");

                Assert.IsTrue(identical);
            }
            #endregion

        }
    }
}
