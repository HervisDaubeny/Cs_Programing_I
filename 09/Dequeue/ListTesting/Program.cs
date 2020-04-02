using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListTesting {
    class Program {
        static void Main(string[] args) {

            List<int> l = new List<int>();
            l.Add(1);
            l.Add(2);
            l.Add(3);
            l.Add(4);
            l.Add(5);
            l.Add(6);
            l.Add(7);
            l.Add(8);
            l.Add(9);
            l.Add(10);

            l.Insert(1, 0);
            print(l);
            l.Insert(1, 2);
            print(l);
            l.Insert(1, 4);
            print(l);
            l.Insert(1, 6);
            print(l);
            l.Insert(1, 8);
            print(l);

            l.Insert(l.Count - 1, 1);
            print(l);
            l.Insert(l.Count - 1, 3);
            print(l);
            l.Insert(l.Count - 1, 5);
            print(l);
            l.Insert(l.Count - 1, 7);
            print(l);
            l.Insert(l.Count - 1, 9);
            print(l);

            Console.WriteLine();
            Console.WriteLine("insert -1 at 0:");
            l.Insert(0, -1);
            print(l);
            Console.WriteLine("insert -2 at count:");
            l.Insert(l.Count, -2);
            print(l);


            int[] array = new int[50];
            l.CopyTo(array, 5);

            foreach (var i in array) {
                Console.WriteLine(i);
            }
            Console.ReadKey();


        }

        static void print(List<int> L) {
            for (int i = 0; i < L.Count; i++) {
                Console.Write("{0} ", L[i]);
            }
            Console.WriteLine();
        }

    }
}
