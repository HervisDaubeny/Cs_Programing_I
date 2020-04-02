using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests {
    [TestClass()]
    public class DequeTests {
        [TestMethod()]
        public void ReverseIndexerTest() {
            Deque<int> D = new Deque<int>() { 1, 2, 3, 4, 5 };
            for (int i = 0; i < D.Count; i++) {
                Assert.AreEqual(D[i], i + 1);
            }

            Deque<int> R = new Deque<int>(D);
            for (int i = 0; i < R.Count; i++) {
                Assert.AreEqual(R[i], R.Count - i);
            }

            D.Reverse();
            for (int i = 0; i < D.Count; i++) {
                Assert.AreEqual(D[i], D.Count - i);
            }
        }

        [TestMethod()]
        public void CopyToTest() {
            List<int> L = new List<int> { 1, 2, 3, 4, 5 };
            Deque<int> D = new Deque<int> { 1, 2, 3, 4, 5 };

            int[] arrayL = new int[10];
            int[] arrayD = new int[10];

            L.CopyTo(arrayL, 3);
            D.CopyTo(arrayD, 3);

            for (int i = 0; i < arrayL.Length; i++) {
                Console.Write("{0} ", arrayL[i]);
            }
            Console.WriteLine();
            for (int i = 0; i < arrayD.Length; i++) {
                Console.Write("{0} ", arrayD[i]);
            }
            Console.WriteLine();

            for (int i = 0; i < arrayL.Length; i++) {
                Assert.AreEqual(arrayL[i], arrayD[i]);
            }
        }
    }
}