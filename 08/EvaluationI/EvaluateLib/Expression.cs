using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluateLib {
    /// <summary>
    /// Object for storing parts of expression tree.
    /// (Actually whole tree recursively.)
    /// Only exists to unify its childern to one type, thus enabling
    /// me to build tree of slightly different elements.
    /// </summary>
    public abstract class Expression {
        public abstract bool GetValue(out int value);

        public static bool TryParse(Stack<string> Stack, bool first, out Expression newExpression) {
            newExpression = default(Expression);
            
            bool result = true;
            int currentNumber = default(int);
            Operation currentOperation = default(Operation);

            if (Stack.Count < 1) {
                return false;
            }
            string currentParse = Stack.Pop();
            //TODO: test the crap out of this
            if (BinaryOperator.TryParse(currentParse, out currentOperation)) {
                Expression firstOperand = default(Expression);
                Expression secondOperand = default(Expression);

                if (!Expression.TryParse(Stack, false, out firstOperand)) {
                    result = false;
                }
                if (!Expression.TryParse(Stack, false, out secondOperand)) {
                    result = false;
                }
                if (result) {
                    newExpression = new BinaryOperator(firstOperand, secondOperand, currentOperation);
                }
            }
            else if (UnaryOperator.TryParse(currentParse, out currentOperation)) {
                Expression operand = default(Expression);

                if (!Expression.TryParse(Stack, false, out operand)) {
                    result = false;
                }
                if (result) {
                    newExpression = new UnaryOperator(operand, currentOperation);
                }
            }
            else if (Number.TryParse(currentParse, out currentNumber)) {
                newExpression = new Number(currentNumber);
            }
            else {
                result = false;
            }

            if (first && Stack.Count > 0) {
                result = false;
            }
            return result;
        }
    }
    
    /// <summary>
    /// Node for storing binary operation and its operands.
    /// </summary>
    public class BinaryOperator : Expression {
        public Expression FirstOperand { get; set; }
        public Expression SecondOperand { get; set; }
        public Operation Operation { get; set; }

        public BinaryOperator(Expression FirstOperand, Expression SecondOperand, Operation Operation) {
            this.FirstOperand = FirstOperand;
            this.SecondOperand = SecondOperand;
            this.Operation = Operation;
        }

        public override bool GetValue(out int value) {
            value = default(int);

            int firstOperand = default(int);
            int secondOperand = default(int);
            if (!this.FirstOperand.GetValue(out firstOperand)) {
                return false;
            }
            if (!this.SecondOperand.GetValue(out secondOperand)) {
                return false;
            }
            if(!BinaryOperator.ComputeValue(firstOperand, secondOperand, this.Operation, out value)) {
                return false;
            }

            return true;
        }

        public static bool ComputeValue(int FistOperand, int SecondOperand, Operation Operation, out int result) {
            result = default(int);
            long checkResult = default(long);


            switch (Operation) {
                case Operation.Addition:
                    result = FistOperand + SecondOperand;
                    checkResult = (long) FistOperand + (long) SecondOperand;
                    if ((long)result != checkResult) {
                        result = -1;
                        return false;
                    }
                    break;
                case Operation.Subtraction:
                    result = FistOperand - SecondOperand;
                    checkResult = (long) FistOperand - (long) SecondOperand;
                    if ((long) result != checkResult) {
                        result = -1;
                        return false;
                    }
                    break;
                case Operation.Multiplication:
                    result = FistOperand * SecondOperand;
                    checkResult = (long) FistOperand * (long) SecondOperand;
                    if ((long) result != checkResult) {
                        result = -1;
                        return false;
                    }
                    break;
                case Operation.Division:
                    if (SecondOperand == 0) {
                        return false;
                    }
                    result = FistOperand / SecondOperand;
                    checkResult = (long) FistOperand / (long) SecondOperand;
                    if ((long) result != checkResult) {
                        result = -1;
                        return false;
                    }
                    break;
                default:
                    throw new Exception("Wrong usage of unary minus");
            }

            return true;
        }

        public static bool TryParse(string RawData, out Operation operation) {
            operation = default(Operation);
            switch (RawData) {
                case "+":
                    operation = Operation.Addition;
                    return true;
                case "-":
                    operation = Operation.Subtraction;
                    return true;
                case "*":
                    operation = Operation.Multiplication;
                    return true;
                case "/":
                    operation = Operation.Division;
                    return true;
                default:
                    //This time it is ok, if nothing above is matched.
                    return false;
            }
        }
    }

    /// <summary>
    /// Node for storing unary operation and its operand.
    /// </summary>
    public class UnaryOperator : Expression {
        public Expression Operand { get; set; }
        public Operation Operation { get; set; }

        public UnaryOperator(Expression Operand, Operation Operation) {
            this.Operand = Operand;
            this.Operation = Operation;
        }

        public override bool GetValue(out int value) {
            value = default(int);
            int operand = default(int);

            if (!this.Operand.GetValue(out operand)) {
                return false;
            }
            if (!UnaryOperator.ComputeValue(operand, this.Operation, out value)) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Computes value of current node.
        /// </summary>
        /// <param name="Operand"></param>
        /// <param name="Operation"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ComputeValue(int Operand, Operation Operation, out int value) {
            value = default(int);
            long checkResult = default(long);

            if (Operation != Operation.CastMinus) {
                throw new Exception("Binary operator used as unary!");
            }
            value = Operand * -1;
            checkResult = (long) Operand * (long) -1;

            if ((long) value != checkResult) {
                value = -1;
                return false;
            }

            return true;
        }

        public static bool TryParse(string RawData, out Operation operation) {
            operation = default(Operation);

            if (RawData == "~") {
                operation = Operation.CastMinus;
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Node for storing int value. (Leaf of expression tree.)
    /// </summary>
    public class Number : Expression {
        public int Value { get; set; }

        public Number(int Value) {
            this.Value = Value;
        }

        public override bool GetValue(out int value) {
            value = this.Value;
            return true;
        }

        public static bool TryParse(string RawData, out int value) {
            value = default(int);

            if (!int.TryParse(RawData, out value)) {
                return false;
            }

            return true;
        }
    }

    public enum Operation { 
        Addition,
        Subtraction,
        Multiplication,
        Division,
        CastMinus
    }
}
