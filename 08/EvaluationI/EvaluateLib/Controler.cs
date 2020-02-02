using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluateLib {
    public class Controler {
        private Stack<string> Stack { get; set; }
        private Expression Expression { get; set; }
        private Error Error { get; set; }
        private int Result { get; set; }
        public Controler() {

        }

        public void GetData() {
            string input = Reader.LineFromConsole();
            ParseData(input);
        }

        public bool IsCorrect() {
            if (Error != default(Error)) {
                return false;
            }
            return true;
        }

        private void ParseData(string RawData) {
            char[] separators = new char[] { ' ' };
            string[] data = RawData.Split(separators);
            Stack<string> stack = new Stack<string>();

            for (int i = data.Length - 1; i >= 0; i--) {
                stack.Push(data[i]);
            }

            Stack = stack;
        }

        public void BuildTree() {
            if (Expression.TryParse(Stack, true, out Expression expression)) {
                Expression = expression;
            }
            else {
                Error = Error.Format;
            }
        }

        public void ComputeTree() {
            int result = default(int);

            if (!Expression.GetValue(out result)) {
                if (result == 0) {
                    Error = Error.Division;
                }
                else if (result == -1) {
                    Error = Error.Overflow;
                }
                else {
                    throw new Exception("Computation returns wrong value on error.");
                }
                return;
            }

            Result = result;
        }

        public void PrintResult() {
            const string formatError = "Format Error";
            const string divisionError = "Divide Error";
            const string overflowError = "Overflow Error";
            switch (Error) {
                case Error.None:
                    Writer.LineToConsole(Result.ToString());
                    break;
                case Error.Format:
                    Writer.LineToConsole(formatError);
                    break;
                case Error.Division:
                    Writer.LineToConsole(divisionError);
                    break;
                case Error.Overflow:
                    Writer.LineToConsole(overflowError);
                    break;
                default:
                    throw new Exception("Just one question... how?");
            }
        }
    }

    public enum Error {
        None,
        Format,
        Division,
        Overflow
    }
}
