using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hervis.Excell;
using System.Runtime.CompilerServices;


[assembly: InternalsVisibleTo("Excell.Tests")]

namespace Excell.Tests {
    internal class Utilities {
        public static bool AreEqual(bool firstItem, bool secondItem ) {
            if (firstItem == secondItem) {
                return true;
            }
            return false;
        }
        public static bool AreEqual( int firstItem, int secondItem ) {
            if (firstItem == secondItem) {
                return true;
            }
            return false;
        }
        public static bool AreEqual( string firstItem, string secondItem ) {
            if (firstItem == secondItem) {
                return true;
            }
            return false;
        }
        public static bool AreEqual( CellType firstItem, CellType secondItem ) {
            if (firstItem == secondItem) {
                return true;
            }
            return false;
        }

        public static bool AreEqual(Coords[] firstArray, Coords[] secondArray ) {
            if (firstArray.Length != secondArray.Length) {
                return false;
            }
            for (int i = 0; i < firstArray.Length; i++) {
                if (firstArray[i].rowCoord != secondArray[i].rowCoord ||
                    firstArray[i].columnCoord != secondArray[i].columnCoord) {
                    return false;
                }
            }
            return true;
        }
    }
}
