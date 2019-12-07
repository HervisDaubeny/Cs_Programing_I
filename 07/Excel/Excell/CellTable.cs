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

        /// <summary>
        /// Count rows.
        /// </summary>
        /// <returns>Int: Number of rows.</returns>
        public int RowCount() {
            return Table.Count;
        }

        /// <summary>
        /// Counts columns of a row.
        /// </summary>
        /// <param name="line">Int: Row to use.</param>
        /// <returns>Int: Number of columns in row.</returns>
        public int ColumnCount(int line) {
            return Table[ line ].Length;
        }

        /// <summary>
        /// Checks if given coords are in range of table.
        /// </summary>
        /// <param name="check">Coords: Coordinates to check.</param>
        /// <returns>True if coordinates are in range of table, false otherwise.</returns>
        public bool AreCoordsValid(Coords check) {
            int noRows = RowCount();

            if (check.rowCoord >= noRows) {
                return false;
            }
            int noCols = ColumnCount(check.rowCoord);

            if (check.columnCoord >= noCols) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Adds new row to table.
        /// </summary>
        /// <param name="Row">Cell[]: Row to be added to table.</param>
        public void AddRow(Cell[] Row) {
            Table.Add(Row);
        }
    }
}


