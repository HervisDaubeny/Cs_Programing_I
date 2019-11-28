using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Excell.Tests")]

namespace Hervis.Excell {
    internal class CellTable {
        private List<Cell[]> Table;
        public CellTable() {
            Table = new List<Cell[]>();
        }

        public Cell this[Coords coords] { 
            get => Table[ coords.rowCoord ][ coords.columnCoord ];
            set => Table[ coords.rowCoord ][ coords.columnCoord ] = value;
        }
                
        public void AddRow(Cell[] Row) {
            Table.Add(Row);
        }
    }
}


