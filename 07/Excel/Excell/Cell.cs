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
        /// Returns true if Cell contains any errorType, false otherwise.
        /// </summary>
        public bool IsError {
            get { return (((int) this._Type) >= 128); }
        }

        /// <summary>
        /// Returns true if Cell contains equation, false otherwise.
        /// </summary>
        public bool IsEquation {
            get { return this.Type == CellType.Equation; }
        }

        /// <summary>
        /// Returns true if Cell contains equation that is part of a cycle, false otherwise.
        /// </summary>
        public bool IsCycle {
            get { return this.Type == CellType.Cycle; }
        }

        /// <summary>
        /// Constructor: creates new cell containing any error or equation.
        /// </summary>
        /// <param name="Type">CellType: type to be set as cells Type. (equation or any error)</param>
        public Cell(CellType Type ) {
            this._Value = 0;
            this._Type = Type;
        }

        /// <summary>
        /// Constructor: creates new cell containing number.
        /// </summary>
        /// <param name="Value">Int: number to be set as cells Value.</param>
        public Cell( int Value ) {
            this._Value = Value;
            this._Type = CellType.Number;
        }

        /// <summary>
        /// Tries to parse data inside Cell.
        /// </summary>
        /// <param name="CellData">String: Data to work with.</param>
        /// <param name="Cell">Cell: created depending on data given.</param>
        /// <returns>New Cell.</returns>
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
