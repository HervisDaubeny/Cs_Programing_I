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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public Cell this[Coords coords] { 
            get => Table[ coords.rowCoord ][ coords.columnCoord ];
            set => Table[ coords.rowCoord ][ coords.columnCoord ] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int RowCount() {
            return Table.Count;
        }

        public int ColumnCount(int line) {
            return Table[ line ].Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public bool AreCoordsValid(Coords check) {
            int noRows = RowCount();
            int noCols = ColumnCount(check.rowCoord);

            if (check.rowCoord >= noRows) {
                return false;
            }
            if (check.columnCoord >= noCols) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Row"></param>
        public void AddRow(Cell[] Row) {
            Table.Add(Row);
        }
    }
}


