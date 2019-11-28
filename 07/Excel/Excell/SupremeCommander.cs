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


        private SupremeCommander() {
            Equations = new Dictionary<Coords, Equation>();
            Data = new CellTable();
        }

        public static SupremeCommander Initialize(TextReader Reader) 
        {
            SupremeCommander thor = new SupremeCommander();

            string line = default(string);
            int numberOfRows = 0;

            while((line = Reader.ReadLine()) != null) {
                string[] cells = line.Split(new char[] { ' ' } , StringSplitOptions.RemoveEmptyEntries);
                Cell[] row = new Cell[ cells.Length ];

                for (int i = 0; i < cells.Length; i++) {
                    row[ i ] = default(Cell);
                    if (Cell.TryParse(cells[ i ], out row[ i ])) {
                        continue;                       
                    }

                    Equation Equation = default(Equation);
                    CellType Error = default(CellType);
                    if(Equation.TryParse(cells[i], out Equation, out Error)) {
                        thor.Equations.Add(new Coords(i, numberOfRows), Equation);
                        row[ i ] = new Cell(CellType.Equation);
                    }
                    else {
                        row[ i ] = new Cell(Error);
                    }
                }
            }
            return thor;
        }    

        public void Calculate() { 
            while(this.Equations.Count > 0) {
                Coords actCord = this.Equations.First().Key;
                KeyValuePair<Coords, Equation> actual = this.Equations.First();
                SolveEquation(actual);
            }
        }

        private void SolveEquation( KeyValuePair<Coords, Equation> eq ) {
            Stack<KeyValuePair<Coords, Equation>> solveStack = new Stack<KeyValuePair<Coords, Equation>>();
            HashSet<Coords> locks = new HashSet<Coords>();
            
            solveStack.Push(eq);

            while(solveStack.Count > 0) {
                KeyValuePair<Coords, Equation> peek = solveStack.Peek();
                if (locks.Contains(solveStack.Peek().Key)) {
                    //TODO: I am cycle
                }
                locks.Add(solveStack.Peek().Key);

                if (this.Data[peek.Value.FirstOperand].IsError) {
                    this.Data[ peek.Key ] = new Cell(CellType.Error);
                    return;
                }
                if (this.Data[ peek.Value.SecondOperand ].IsError) {
                    this.Data[ peek.Key ] = new Cell(CellType.Error);
                    return;
                }
                if (this.Data[ peek.Value.FirstOperand ].IsEquation) {
                    solveStack.Push(new KeyValuePair<Coords, Equation>(
                        peek.Value.FirstOperand,
                        this.Equations[ peek.Value.FirstOperand ]));
                    continue;
                }
                if (this.Data[ peek.Value.SecondOperand ].IsEquation) {
                    solveStack.Push(new KeyValuePair<Coords, Equation>(
                        peek.Value.SecondOperand,
                        this.Equations[ peek.Value.SecondOperand ]));
                    continue;
                }
                int a = this.Data[ peek.Value.FirstOperand ].Value;
                int b = this.Data[ peek.Value.FirstOperand ].Value;
                int res = a + b; //TODO: switch operator
                this.Data[ peek.Key ] = new Cell(res);
                solveStack.Pop();
                locks.Remove(peek.Key);
            }
        }
    }
}
