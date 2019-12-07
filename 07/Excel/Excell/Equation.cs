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

        /// <summary>
        /// Tries to parse data given to valid equation.
        /// </summary>
        /// <param name="EquationData">String: Data to work with.</param>
        /// <param name="Equation">Equation: Contains valid Equation or null.</param>
        /// <param name="Error">CellType: Contains Equation or errorType found during parsing.</param>
        /// <returns>On succes valid new Equation and corrsponding CellType. Empty Equation and corresponding error as CellType.</returns>
        public static bool TryParse(string EquationData, out Equation Equation, out CellType Error) {
            Equation = default(Equation);
            Error = default(CellType);
            const int alphabetLength = 26;

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
                for (int j = 0; j < splitedData[i].Length; j++) {
                    if (splitedData[i][j] >= (int)'A' && splitedData[i][j] <= (int)'Z') {
                        columnCoord *= alphabetLength;
                        columnCoord += (int) splitedData[ i ][ j ] - 64;
                    }
                    else if (splitedData[i][j] >= (int)'0' && splitedData[i][j] <= (int)'9') {
                        if (int.TryParse(splitedData[ i ].Substring(j), out rowCoord)) {
                            Operand[ i - 1 ] = new Coords(rowCoord - 1, columnCoord - 1);
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

    /// <summary>
    /// Structure representing coordinates to cells in CellTable.
    /// </summary>
    internal struct Coords {
        public readonly int rowCoord;
        public readonly int columnCoord;

        public Coords(int rowCoord , int columnCoord) {
            this.rowCoord = rowCoord;
            this.columnCoord = columnCoord;
        }

        public static bool operator == (Coords firstCoords, Coords secondCoords ) {
            if (firstCoords.columnCoord == secondCoords.columnCoord &&
                firstCoords.rowCoord == secondCoords.rowCoord) {
                return true;
            }
            return false;
        }

        public static bool operator != (Coords firstCoords, Coords secondCoords ) {
            if (firstCoords.columnCoord != secondCoords.columnCoord ||
                firstCoords.rowCoord != secondCoords.rowCoord) {
                return true;
            }
            return false;
        }
    }

    internal enum Operation : Byte {
        multiplication = 0x2A,
        addition = 0x2B,
        subtraction = 0x2D,
        division = 0x2F
    }
}
