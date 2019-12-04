using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hervis.Excell;
using System.IO;

namespace Excell.Tests {
    [TestClass]
    public class FullTests {
        [TestMethod]
        public void EmptyFile() {
            
        }

        [TestMethod]
        public void ReguarFile() {

        }

        [TestMethod]
        public void BigFile() {
            Call(new string[] { 
                $@"{Environment.CurrentDirectory}\Files\BigIn.in",
                $@"{Environment.CurrentDirectory}\Files\File.out"
            });
        }


        private void Call(string[] args) {
            TextReader reader = default(TextReader);
            TextWriter writer = default(TextWriter);
            SupremeCommander Thor = default(SupremeCommander);

            if (args.Length != 2) {
                Console.WriteLine("Argument Error");
                return;
            }
            try {
                reader = File.OpenText(args[ 0 ]);
                writer = File.CreateText(args[ 1 ]);
            }
            catch (Exception) {
                Console.WriteLine("File Error");
                return;
            }

            Thor = SupremeCommander.Initialize(reader, writer);
            Thor.SolveEquations();
            Thor.WriteFile();
            Thor.Dispose();
        }
    }
}
