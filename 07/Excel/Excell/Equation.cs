using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Excell.Tests")]

namespace Hervis.Excell {
    internal struct Equation {
        public Coords FirstOperand;
        public Coords SecondOperand;
        public Operation Operation;

        public static bool TryParse(string EquationData, out Equation Equation, out CellType Error) {
            Equation = default(Equation);
            Error = default(CellType);

            char[] Operators = { '=', '+', '-', '*', '/' };

            if (EquationData[0] != '=') {
                Error = CellType.Invalid;
                return false;
            }
            string[] splitedData = EquationData.Split(Operators);

            if (splitedData.Length < 3) {
                // Missing operation
                Error = CellType.MissingOperation;
                return false;
            }
            if (splitedData.Length > 3) {
                // Too many operations
                Error = CellType.Formula;
                return false;
            }
            if (splitedData[1].Length == 0 || splitedData[2].Length == 0) {
                // Operator length = 0
                Error = CellType.Formula;
            }
            Coords[] Operand = new Coords[ 2 ];

            for (int i = 1; i < 3; i++) {
                int rowCoord = -1;
                int columnCoord = 0;
                //TODO: use 26 as base of folowing counting
                for (int j = 0; j < splitedData[i].Length; j++) {
                    if (splitedData[i][j] > 64 && splitedData[i][j] < 93) {
                        columnCoord *= 27;
                        columnCoord += (int) splitedData[ i ][ j ] - 64;
                    }
                    else if (splitedData[i][j] > 47 && splitedData[i][j] < 58) {
                        if (int.TryParse(splitedData[ i ].Substring(j), out rowCoord)) {
                            Operand[ i - 1 ] = new Coords(rowCoord, columnCoord);
                            break;
                        }
                        else {
                            Error = CellType.Formula;
                            return false;
                        }
                    }
                    else {
                        Error = CellType.Formula;
                        return false;
                    }
                }
                if (rowCoord < 0) {
                    Error = CellType.Formula;
                    return false;
                }
            }
            Equation.FirstOperand = Operand[ 0 ];
            Equation.SecondOperand = Operand[ 1 ];
            Equation.Operation = (Operation) EquationData[ splitedData[ 1 ].Length + 1 ];
            return true;
        }
    }

    internal struct Coords {
        public readonly int rowCoord;
        public readonly int columnCoord;

        public Coords(int rowCoord , int columnCoord) {
            this.rowCoord = rowCoord;
            this.columnCoord = columnCoord;
        }
    }
    internal enum Operation : Byte {
        addition = 0x2F,
        subtraction = 0x2D,
        multiplication = 0x2A,
        division = 0x2F
    }
}
