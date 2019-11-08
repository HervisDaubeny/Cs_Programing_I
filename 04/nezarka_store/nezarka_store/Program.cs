using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nezarka_store {
    class Program {
        /// <summary>
        /// This method takes care of launching the actual program.
        /// It creates instances of objects needed in the end it disposes them.
        /// Also it shuts the program down in case of Data error.
        /// </summary>
        /// <param name="args"></param>
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
            storeViewer.Dispose();
        }
    }
}
