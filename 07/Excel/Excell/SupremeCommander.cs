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
        /// 
        /// </summary>
        private SupremeCommander() {
            Equations = new Dictionary<Coords, Equation>();
            Data = new CellTable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Reader"></param>
        /// <returns></returns>
        public static SupremeCommander Initialize(TextReader Reader, TextWriter Writer) 
        {
            SupremeCommander thor = new SupremeCommander();
            thor.Writer = Writer;

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
        /// 
        /// </summary>
        public void SolveEquations() { 
            while(this.Equations.Count > 0) {
                KeyValuePair<Coords, Equation> actual = this.Equations.First();
                SolveEquation(actual);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void WriteFile() {
            WriteDataToFile();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Equation"></param>
        private void SolveEquation( KeyValuePair<Coords, Equation> Equation ) {
            Stack<KeyValuePair<Coords, Equation>> solveStack = new Stack<KeyValuePair<Coords, Equation>>();
            HashSet<Coords> locks = new HashSet<Coords>();
            //HashSet<Lock> locks = new HashSet<Lock>();
            
            solveStack.Push(Equation);
            Equation.Value.SaveStackInfo(solveStack.Count);

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
                //TODO: add stack size to locks PROB. cant use dictionary because of uniqe values requirement
                if (locks.Contains(currentKey) && solveStack.Count != currentEquation.StackInfo) {
                    RemoveCycle(solveStack, locks, currentKey);
                    continue;
                }
                locks.Add(currentKey);

                if (firstOperand.IsError || secondOperand.IsError) {
                    this.Data[ currentKey ] = new Cell(CellType.Error);
                    this.Equations.Remove(currentKey);
                    locks.Remove(currentKey);
                    return;
                }
                if (firstOperand.IsEquation) {
                    Coords operandCoords = currentEquation.FirstOperand;
                    Equation operandEquation = this.Equations[ operandCoords ];
                    operandEquation.SaveStackInfo(solveStack.Count);

                    solveStack.Push(new KeyValuePair<Coords, Equation>(operandCoords, operandEquation));
                    continue;
                }
                if (secondOperand.IsEquation) {
                    Coords operandCoords = currentEquation.SecondOperand;
                    Equation operandEquation = this.Equations[ operandCoords ];
                    operandEquation.SaveStackInfo(solveStack.Count);
                    
                    solveStack.Push(new KeyValuePair<Coords, Equation>(operandCoords, operandEquation));
                    continue;
                }

                this.Data[ currentKey ] = TryCompute(firstOperand.Value, secondOperand.Value, currentEquation.Operation);
                this.Equations.Remove(currentKey);
                solveStack.Pop();
                locks.Remove(currentKey);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void WriteDataToFile() {
            for (int row = 0; row < Data.RowCount(); row++) {
                StringBuilder line = new StringBuilder();

                for (int column = 0; column < Data.ColumnCount(row); column++) {
                    Coords coodrsCellToWrite = new Coords(row, column);
                    Cell cellToWrite = Data[ coodrsCellToWrite ];

                    if (column > 0) {
                        line.Append(" ");
                    }
                    switch (cellToWrite.Type) {
                        case CellType.Empty:
                            line.Append("[]");
                            break;
                        case CellType.Number:
                            line.Append(cellToWrite.Value);
                            break;
                        case CellType.Invalid:
                            line.Append("#INVVAL");
                            break;
                        case CellType.Error:
                            line.Append("#ERROR");
                            break;
                        case CellType.ZeroDivision:
                            line.Append("#DIV0");
                            break;
                        case CellType.Cycle:
                            line.Append("#CYCLE");
                            break;
                        case CellType.MissingOperation:
                            line.Append("#MISSOP");
                            break;
                        case CellType.Formula:
                            line.Append("#FORMULA");
                            break;
                        default:
                            throw new Exception("You tried to print an equation type of cell!");
                    }
                }

                Writer.WriteLine(line.ToString());
                Writer.Flush();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="SolveStack"></param>
        /// <param name="Locks"></param>
        /// <param name="Key"></param>
        private void RemoveCycle( Stack<KeyValuePair<Coords, Equation>> SolveStack, HashSet<Coords> Locks, Coords Key ) {
            Coords currentKey = default(Coords);
            SolveStack.Pop(); //Avoids marking first item as cycle twice.

            bool inCycle = true;
            //bool firstIteration = true;
            do {
                currentKey = SolveStack.Pop().Key;
                if (currentKey == Key) {// && !firstIteration) {
                    inCycle = false;
                }
                this.Data[ currentKey ] = new Cell(CellType.Cycle);
                this.Equations.Remove(currentKey);
                Locks.Remove(currentKey);
                //firstIteration = false;
            } while (inCycle);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
            this.Data = null;
            this.Equations = null;
            this.Writer = null;
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
