using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace nezarka_store.unit_tests {

    [TestClass]
    public class CommandParsingTests {

        SupremeCommander Heimdal;
        StoreViewer Overseer;
        TextReader Reader;
        StreamWriter Writer;

        [TestMethod]
        public void ParseCommand_AddNotExistingBook_InvalidCommand() {


            const string inFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\AddNotExistingBookTest.in";
            const string correctFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\AddNotExistingBookExpected.out";
            const string outFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\AddNotExistingBookTest.out";

            Reader = File.OpenText(inFile);
            Heimdal = new SupremeCommander();
            Writer = new StreamWriter(new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            Overseer = new StoreViewer(Writer);

            Heimdal.GetData(Reader, out bool wrongInput);
            Heimdal.InititateCommandProcessing(Reader, Overseer);

            Heimdal.DisposeStore();
            Writer.Close();

            bool areEqual = Utils.FileDiff(outFile, correctFile);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void ParseCommand_GetNotExistingBookDetails_InvalidCommand() {


            const string inFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\GetNotExistingBookDetailsTest.in";
            const string correctFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\GetNotExistingBookDetailsExpected.out";
            const string outFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\GetNotExistingBookDetailsTest.out";

            Reader = File.OpenText(inFile);
            Heimdal = new SupremeCommander();
            Writer = new StreamWriter(new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            Overseer = new StoreViewer(Writer);

            Heimdal.GetData(Reader, out bool wrongInput);
            Heimdal.InititateCommandProcessing(Reader, Overseer);

            Heimdal.DisposeStore();
            Writer.Close();

            bool areEqual = Utils.FileDiff(outFile, correctFile);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void ParseCommand_IlegalBookId_InvalidCommand() {


            const string inFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\IlegalBookIdTest.in";
            const string correctFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\IlegalBookIdExpected.out";
            const string outFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\IlegalBookIdTest.out";

            Reader = File.OpenText(inFile);
            Heimdal = new SupremeCommander();
            Writer = new StreamWriter(new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            Overseer = new StoreViewer(Writer);

            Heimdal.GetData(Reader, out bool wrongInput);
            Heimdal.InititateCommandProcessing(Reader, Overseer);

            Heimdal.DisposeStore();
            Writer.Close();

            bool areEqual = Utils.FileDiff(outFile, correctFile);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void ParseCommand_IlegalUrlPart_InvalidCommand() {


            const string inFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\IlegalUrlPartTest.in";
            const string correctFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\IlegalUrlPartExpected.out";
            const string outFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\IlegalUrlPartTest.out";

            Reader = File.OpenText(inFile);
            Heimdal = new SupremeCommander();
            Writer = new StreamWriter(new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            Overseer = new StoreViewer(Writer);

            Heimdal.GetData(Reader, out bool wrongInput);
            Heimdal.InititateCommandProcessing(Reader, Overseer);

            Heimdal.DisposeStore();
            Writer.Close();

            bool areEqual = Utils.FileDiff(outFile, correctFile);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void ParseCommand_IlegalUserId_InvalidCommand() {


            const string inFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\IlegalUserIdTest.in";
            const string correctFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\IlegalUserIdExpected.out";
            const string outFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\IlegalUserIdTest.out";

            Reader = File.OpenText(inFile);
            Heimdal = new SupremeCommander();
            Writer = new StreamWriter(new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            Overseer = new StoreViewer(Writer);

            Heimdal.GetData(Reader, out bool wrongInput);
            Heimdal.InititateCommandProcessing(Reader, Overseer);

            Heimdal.DisposeStore();
            Writer.Close();

            bool areEqual = Utils.FileDiff(outFile, correctFile);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void ParseCommand_RemoveNotExistingBook_InvalidCommand() {


            const string inFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\RemoveNotExistingBookTest.in";
            const string correctFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\RemoveNotExistingBookExpected.out";
            const string outFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\RemoveNotExistingBookTest.out";

            Reader = File.OpenText(inFile);
            Heimdal = new SupremeCommander();
            Writer = new StreamWriter(new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            Overseer = new StoreViewer(Writer);

            Heimdal.GetData(Reader, out bool wrongInput);
            Heimdal.InititateCommandProcessing(Reader, Overseer);

            Heimdal.DisposeStore();
            Writer.Close();

            bool areEqual = Utils.FileDiff(outFile, correctFile);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void ParseCommand_UrlCaseSensitivity_InvalidCommand() {


            const string inFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\UrlCaseSensitivityTest.in";
            const string correctFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\UrlCaseSensitivityExpected.out";
            const string outFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\UrlCaseSensitivityTest.out";

            Reader = File.OpenText(inFile);
            Heimdal = new SupremeCommander();
            Writer = new StreamWriter(new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            Overseer = new StoreViewer(Writer);

            Heimdal.GetData(Reader, out bool wrongInput);
            Heimdal.InititateCommandProcessing(Reader, Overseer);

            Heimdal.DisposeStore();
            Writer.Close();

            bool areEqual = Utils.FileDiff(outFile, correctFile);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void ParseCommand_WrongKeyword_InvalidCommand() {


            const string inFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\WrongKeywordTest.in";
            const string correctFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\WrongKeywordExpected.out";
            const string outFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\WrongKeywordTest.out";

            Reader = File.OpenText(inFile);
            Heimdal = new SupremeCommander();
            Writer = new StreamWriter(new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            Overseer = new StoreViewer(Writer);

            Heimdal.GetData(Reader, out bool wrongInput);
            Heimdal.InititateCommandProcessing(Reader, Overseer);

            Heimdal.DisposeStore();
            Writer.Close();

            bool areEqual = Utils.FileDiff(outFile, correctFile);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void ParseCommand_WrongUrlSuffix_InvalidCommand() {


            const string inFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\WrongUrlSuffixTest.in";
            const string correctFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\WrongUrlSuffixExpected.out";
            const string outFile = "D:\\Programing\\Cs_Programming_I\\04\\nezarka_store\\test_files\\CommandParsingTests\\WrongUrlSuffixTest.out";

            Reader = File.OpenText(inFile);
            Heimdal = new SupremeCommander();
            Writer = new StreamWriter(new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            Overseer = new StoreViewer(Writer);

            Heimdal.GetData(Reader, out bool wrongInput);
            Heimdal.InititateCommandProcessing(Reader, Overseer);

            Heimdal.DisposeStore();
            Writer.Close();

            bool areEqual = Utils.FileDiff(outFile, correctFile);

            Assert.IsTrue(areEqual);
        }
    }
}
