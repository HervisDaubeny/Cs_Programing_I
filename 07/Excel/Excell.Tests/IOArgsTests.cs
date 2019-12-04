using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hervis.Excell;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Excell.Tests {
    [TestClass]
    public class IOArgsTests {
        [TestMethod]
        public void ArgumentCheckGetsZeroArguments() {
            string[] args = new string[] { };
            bool testFailed = false;

            if (args.Length != 2) {
                Console.WriteLine("Argument Error");
                testFailed = true;
            }
            Assert.IsTrue(testFailed);
        }

        [TestMethod]
        public void ArgumentCheckGetsTooManyArguments() {
            string[] args = new string[] { "kjkalsdjfamsdfhmklh", "kjfaklsjdfl",
                "kadfj;askl.asdkfjasdkl", "kddkasjf;klsd", "kasdfjkl" };
            bool testFailed = false;

            if (args.Length != 2) {
                Console.WriteLine("Argument Error");
                testFailed = true;
            }
            Assert.IsTrue(testFailed);
        }

        [TestMethod]
        public void InputFileIsReadable()
        {
            string[] args = new string[] { "D:\\Programing\\Cs_Programming_I\\07\\Excel\\TestData\\dummy.sheet",
            "D:\\Programing\\Cs_Programming_I\\07\\Excel\\TestData\\dummy.eval"};
            TextReader reader = default(TextReader);
            TextWriter writer = default(TextWriter);
            bool result = true;

            if (args.Length != 2) {
                Console.WriteLine("Argument Error");
                result = false;
            }
            try {
                reader = File.OpenText(args[ 0 ]);
                writer = File.CreateText(args[ 1 ]);
            }
            catch (Exception) {
                Console.WriteLine("File Error");
                result = false;
            }
            Assert.IsTrue(result);
        }
    }
}
