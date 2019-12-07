using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hervis.Excell {
    public class SupremeCommander {
        Dictionary<Coords, Equation> Equations;
        CellTable Data;
        TextWriter Writer;

        /// <summary>
        /// Creates new SupremeComander object.
        /// </summary>
        private SupremeCommander(TextWriter Writer) {
            Equations = new Dictionary<Coords, Equation>();
            Data = new CellTable();
            this.Writer = Writer;

        }

        /// <summary>
        /// Creates new SupremeComander and reades input file to commander's database.
        /// </summary>
        /// <param name="Reader">TextReader object, pointing to input file.</param>
        /// <returns>New SupremeCommander object.</returns>
        public static SupremeCommander Initialize(TextReader Reader, TextWriter Writer) 
        {
            SupremeCommander thor = new SupremeCommander(Writer);

            string line = default(string);
            int numberOfRows = 0;

            while((line = Reader.ReadLine()) != null) {
                string[] cells = line.Split(new char[] { ' ' } , StringSplitOptions.RemoveEmptyEntries);
                Cell[] row = new Cell[ cells.Length ];

                for (int column = 0; column < cells.Length; column++) {
                    row[ column ] = default(Cell);
                    if (Cell.TryParse(cells[ column ], out row[ column ])) {
                        continue;                       
                    }

                    Equation Equation = default(Equation);
                    CellType Error = default(CellType);
                    if(Equation.TryParse(cells[column], out Equation, out Error)) {
                        thor.Equations.Add(new Coords(numberOfRows, column), Equation);
                        row[ column ] = new Cell(CellType.Equation);
                    }
                    else {
                        row[ column ] = new Cell(Error);
                    }
                }
                thor.Data.AddRow(row);
                numberOfRows++;
            }
            return thor;
        }

        /// <summary>
        /// Write data to output file. If equation is encountered, it is solved and then written.
        /// </summary>
        public void WriteAndSolve() {
            const string empty = "[]";
            const string invalid = "#INVVAL";
            const string error = "#ERROR";
            const string zeroDivision = "#DIV0";
            const string cycle = "#CYCLE";
            const string missingOp = "#MISSOP";
            const string formula = "#FORMULA";
            const string space = " ";

            int rowCount = this.Data.RowCount();
            for (int row = 0; row < rowCount; row++) {
                StringBuilder line = new StringBuilder();

                int columnCount = this.Data.ColumnCount(row);
                for (int column = 0; column < columnCount; column++) {
                    Coords currentCoords = new Coords(row, column);
                    Cell currentCell = this.Data[currentCoords];
                    if (currentCell.IsEquation) {
                        KeyValuePair<Coords, Equation> currentEquation 
                            = new KeyValuePair<Coords,Equation>(currentCoords, this.Equations[currentCoords]);
                        SolveEquation(currentEquation);
                        currentCell = this.Data[currentCoords];
                    }

                    if (column > 0) {
                        line.Append(space);
                    }
                    switch (currentCell.Type) {
                        case CellType.Empty:
                            line.Append(empty);
                            break;
                        case CellType.Number:
                            line.Append(currentCell.Value);
                            break;
                        case CellType.Invalid:
                            line.Append(invalid);
                            break;
                        case CellType.Error:
                            line.Append(error);
                            break;
                        case CellType.ZeroDivision:
                            line.Append(zeroDivision);
                            break;
                        case CellType.Cycle:
                            line.Append(cycle);
                            break;
                        case CellType.MissingOperation:
                            line.Append(missingOp);
                            break;
                        case CellType.Formula:
                            line.Append(formula);
                            break;
                        default:
                            throw new Exception("You tried to print an equation typed cell!");
                    }
                }

                Writer.WriteLine(line.ToString());
            }
        }

        /// <summary>
        /// Solves given equation and saves correct value and type to cell, holding named equation.
        /// </summary>
        /// <param name="Equation">KeyValuePair[Coords, Equation] containing coordinates to cell and equation itself.</param>
        private void SolveEquation( KeyValuePair<Coords, Equation> Equation ) {
            Stack<KeyValuePair<Coords, Equation>> solveStack = new Stack<KeyValuePair<Coords, Equation>>();
            HashSet<Coords> locks = new HashSet<Coords>();
            
            solveStack.Push(Equation);

            while(solveStack.Count > 0) {
                KeyValuePair<Coords, Equation> peek = solveStack.Peek();
                Coords currentKey = peek.Key;
                Equation currentEquation = peek.Value;
                Cell firstOperand = default(Cell);
                Cell secondOperand = default(Cell);
                Coords firstOperandCoords = currentEquation.FirstOperand;
                Coords secondOperandCoords = currentEquation.SecondOperand;

                if (this.Data.AreCoordsValid(firstOperandCoords)) {
                    firstOperand = this.Data[ currentEquation.FirstOperand ];
                }
                if (this.Data.AreCoordsValid(secondOperandCoords)) {
                    secondOperand = this.Data[ currentEquation.SecondOperand ];
                }
                if (locks.Contains(currentKey)) {
                    RemoveCycle(solveStack, locks, currentKey);
                    RemoveError(solveStack, locks);
                    continue;
                }
                locks.Add(currentKey);

                if (firstOperand.IsError || secondOperand.IsError) {
                    this.Data[ currentKey ] = new Cell(CellType.Error);
                    locks.Remove(currentKey);
                    return;
                }
                if (firstOperand.IsEquation) {
                    Coords operandCoords = currentEquation.FirstOperand;
                    Equation operandEquation = this.Equations[ operandCoords ];

                    solveStack.Push(new KeyValuePair<Coords, Equation>(operandCoords, operandEquation));
                    continue;
                }
                if (secondOperand.IsEquation) {
                    Coords operandCoords = currentEquation.SecondOperand;
                    Equation operandEquation = this.Equations[ operandCoords ];
                    
                    solveStack.Push(new KeyValuePair<Coords, Equation>(operandCoords, operandEquation));
                    continue;
                }

                this.Data[ currentKey ] = TryCompute(firstOperand.Value, secondOperand.Value, currentEquation.Operation);
                solveStack.Pop();
                locks.Remove(currentKey);
                if (locks.Count > 0) {
                    locks.Remove(solveStack.Peek().Key);
                }
            }
        }

        /// <summary>
        /// Computes equation if all data are correct. Otherwise sets CellType to corresponding error type.
        /// </summary>
        /// <param name="firstOperand">Coords pointing to first equation operand.</param>
        /// <param name="secondOperand">Coords pointing to second equation operand.</param>
        /// <param name="operation">Operation to be done upon operands.</param>
        /// <returns>Cell with correct type and value.</returns>
        private Cell TryCompute(int firstOperand, int secondOperand, Operation operation) {
            switch (operation) {
                case Operation.addition:
                    return new Cell(firstOperand + secondOperand);

                case Operation.subtraction:
                    return new Cell(firstOperand - secondOperand);

                case Operation.multiplication:
                    return new Cell(firstOperand * secondOperand);

                case Operation.division:
                    if (secondOperand == 0) {
                        return new Cell(CellType.ZeroDivision);
                    }
                    return new Cell(firstOperand / secondOperand);

                default:
                    throw new Exception("What the heck you just tried to use as operation?!");
            }
        }

        /// <summary>
        /// Removes cycle from stack, when solving nested equations.
        /// </summary>
        /// <param name="SolveStack">SolveStack with equations.</param>
        /// <param name="Locks">HashSet with currently computed equations.</param>
        /// <param name="Key">Coords of equation nested deepest, marking end the cycle.</param>
        private void RemoveCycle( Stack<KeyValuePair<Coords, Equation>> SolveStack, HashSet<Coords> Locks, Coords Key ) {
            Coords currentKey = SolveStack.Pop().Key;
            this.Data[ currentKey ] = new Cell(CellType.Cycle);

            bool inCycle = true;
            while (inCycle) {
                currentKey = SolveStack.Pop().Key;
                if (this.Data[ currentKey ].IsCycle) {
                    inCycle = false;
                }
                else {
                    this.Data[ currentKey ] = new Cell(CellType.Cycle);
                }

                //this.Equations.Remove(currentKey);
                Locks.Remove(currentKey);
            }
        }

        /// <summary>
        /// Removes error from stack. In other words, sets all equations from stac as error. Called after removing a cycle.
        /// </summary>
        /// <param name="SolveStack">SolveStack with equations.</param>
        /// <param name="Locks">HashSet with currently computed equations.</param>
        private void RemoveError( Stack<KeyValuePair<Coords,Equation>> SolveStack, HashSet<Coords> Locks ) {
            Coords currentKey = default(Coords);

            while (SolveStack.Count > 0) {
                currentKey = SolveStack.Pop().Key;
                this.Data[ currentKey ] = new Cell(CellType.Error);
                //this.Equations.Remove(currentKey);
                Locks.Remove(currentKey);
            }
        }

        /// <summary>
        /// Correctly closes writer and gets rid of all objects, properties.
        /// </summary>
        public void Dispose() {
            this.Data = null;
            this.Equations = null;
            this.Writer.Flush();
            this.Writer.Close();
            this.Writer.Dispose();
        }
    }

    internal struct Lock {
        public readonly int StackSize;
        public readonly Coords Coords;

        public Lock(Coords Coords, int StackSize) {
            this.Coords = Coords;
            this.StackSize = StackSize;
        }
    }
}
