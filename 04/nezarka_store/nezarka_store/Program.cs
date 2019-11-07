using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nezarka_store {
    class Program {
        static void Main( string[] args ) {

            StoreViewer storeViewer = new StoreViewer(Console.Out);
            SupremeCommander Thor = new SupremeCommander();
            bool wrongStoreInput;

            Thor.GetData(Console.In, out wrongStoreInput);

            if (wrongStoreInput) {

                Thor.PrintDataError();

                return;
            }

            Thor.InititateCommandProcessing(Console.In, storeViewer);

            Thor.DisposeStore();
        }
    }
}
