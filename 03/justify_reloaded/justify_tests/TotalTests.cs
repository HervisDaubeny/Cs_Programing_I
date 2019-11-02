using justify_reloaded;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace justify_tests {

    [TestClass]
    public class TotalTests {

        [TestMethod]
        public void Test01() {

            RedirectedConsole.Init();
            RedirectedConsole.SetOut("out.cons");

            Call("plain.txt format.txt abc");
            Call("plain.txt format.txt 10");

            RedirectedConsole.ResetAll();

        }


        public void Call( string args ) {

            Program.Main(args.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries));
        }



        public static class RedirectedConsole {

            private static TextReader DefaultIn;
            private static TextWriter DefaultOut;
            private static TextWriter DefaultErr;

            private static FileStream FsIn;
            private static FileStream FsOut;
            private static FileStream FsErr;

            private static StreamWriter WrOut;
            private static StreamWriter WrErr;
            private static StreamReader RdIn;

            public static bool RedirectedIn { get; private set; }
            public static bool RedirectedOut { get; private set; }
            public static bool RedirectedErr { get; private set; }

            /// <summary>
            /// Set default I/O streams
            /// </summary>
            static RedirectedConsole() {

                Init();
            }

            public static void Init() {

                DefaultIn = Console.In;
                DefaultOut = Console.Out;
                DefaultErr = Console.Error;
            }

            public static void ResetAll() { 
                
                ResetIn();
                ResetOut();
                ResetErr();
            }

            public static void ResetIn() {
                if (RedirectedIn) {

                    RdIn.Close();
                    RdIn.Dispose();
                    RdIn = null;
                    FsIn.Close();
                    FsIn.Dispose();
                    FsIn = null;

                    Console.SetIn(DefaultIn);
                    RedirectedIn = false;
                }
            }            

            public static void ResetOut() {

                if (RedirectedOut) {

                    WrOut.Flush();
                    WrOut.Close();
                    WrOut.Dispose();
                    WrOut = null;
                    FsOut.Close();
                    FsOut.Dispose();
                    FsOut = null;

                    Console.SetOut(DefaultOut);
                    RedirectedOut = false;
                }
            }

            public static void ResetErr() {
                if (RedirectedOut) {

                    WrErr.Flush();
                    WrErr.Close();
                    WrErr.Dispose();
                    WrErr = null;
                    FsErr.Close();
                    FsErr.Dispose();
                    FsErr = null;

                    Console.SetError(DefaultErr);
                    RedirectedErr = false;
                }
            }

            public static bool SetOut(string Path ) {

                ResetOut();
                try {

                    FsOut = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write);
                    WrOut = new StreamWriter(FsOut);
                }
                catch {

                    return false;
                }

                Console.SetOut(WrOut);
                RedirectedOut = true;
                return true;
            }

            public static bool SetErr( string Path ) {

                ResetErr();
                try {

                    FsErr = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write);
                    WrErr = new StreamWriter(FsErr);
                }
                catch {

                    return false;
                }

                Console.SetError(WrErr);
                RedirectedErr = true;
                return true;
            }

            public static bool SetIn( string Path ) {

                ResetIn();
                try {

                    FsIn = new FileStream(Path, FileMode.Open, FileAccess.In);
                    RdIn = new StreamReader(FsErr);
                }
                catch {

                    return false;
                }

                Console.SetIn(RdIn);
                RedirectedIn = true;
                return true;
            }


        }

    }
}
