using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hervis.Excell {
    internal struct Cell {
        private CellType _Type;
        private int _Value;

        public CellType Type { get => _Type; set => _Type = value; }
        public int Value { 
            get => _Value;
            set { 
                if(this.Type == CellType.Invalid)
                    throw new ArgumentException("You are a fool, Jashinto!");
                _Value = value;
                this.Type = CellType.Number;
            } 
        }

        public bool IsError {
            get { return (((int) this._Type) >= 128); }
        }

        public bool IsEquation {
            get { return true; }
        }

        public bool IsValue {
            get { return this.Type == CellType.Number; }
        }

        public Cell(CellType Type ) {
            this._Value = 0;
            this._Type = Type;
        }

        public Cell( int Value ) {
            this._Value = Value;
            this._Type = CellType.Number;
        }

        public static bool TryParse( string CellData, out Cell Cell ) {

            Cell = default;

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
