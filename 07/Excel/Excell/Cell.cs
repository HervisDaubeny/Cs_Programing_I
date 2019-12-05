using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hervis.Excell {
    internal struct Cell {
        private CellType _Type;
        private int _Value;
        public CellType Type { 
            get => _Type;
            set => _Type = value; 
        }
        public int Value { 
            get => _Value;
            set {
                if (this.Type == CellType.Invalid) {
                    throw new ArgumentException("You are a fool, Jacinto! All ancestors of this cell were errors.");
                }
                _Value = value;
                this.Type = CellType.Number;
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsError {
            get { return (((int) this._Type) >= 128); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEquation {
            get { return this.Type == CellType.Equation; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCycle {
            get { return this.Type == CellType.Cycle; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        public Cell(CellType Type ) {
            this._Value = 0;
            this._Type = Type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Cell( int Value ) {
            this._Value = Value;
            this._Type = CellType.Number;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CellData"></param>
        /// <param name="Cell"></param>
        /// <returns></returns>
        public static bool TryParse( string CellData, out Cell Cell ) {

            Cell = default(Cell);

            if (CellData == "[]") {

                Cell.Type = CellType.Empty;
                return true;
            }
            if (int.TryParse(CellData, out int value)) {
                Cell.Value = value;
                Cell.Type = CellType.Number;
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal enum CellType : Byte {
        Empty,
        Number,
        Equation,
        Invalid = 128,
        Error,
        ZeroDivision,
        Cycle,
        MissingOperation,
        Formula
    }
}
