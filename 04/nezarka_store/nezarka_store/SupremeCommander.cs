using NezarkaBookstore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nezarka_store {
    class SupremeCommander {

        public bool GetData() {

            bool wrongInput;
            ModelStore.LoadFrom(Console.In, out wrongInput);

            return wrongInput;
        }

        public void PrintDataError() {

            Console.WriteLine("Data error.");
        }
    }
}
