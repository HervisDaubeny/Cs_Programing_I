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
    class Expression {
        public static bool TryParse(Stack<string> Stack, out Expression newExpression) {
            newExpression = default(Expression);
            
            bool result = true;
            int currentNumber = default(int);
            Operation currentOperation = default(Operation);
            //TODO: check stack size
            string currentParse = Stack.Pop();
            //TODO: test the crap out of this
            if (BinaryOperator.TryParse(currentParse, out currentOperation)) {
                Expression firstOperand = default(Expression);
                Expression secondOperand = default(Expression);

                if (!Expression.TryParse(Stack, out firstOperand)) {
                    result = false;
                }
                if (!Expression.TryParse(Stack, out secondOperand)) {
                    result = false;
                }
                if (result) {
                    newExpression = new BinaryOperator(firstOperand, secondOperand, currentOperation);
                }
            }
            else if (UnaryOperator.TryParse(currentParse, out currentOperation)) {
                Expression operand = default(Expression);

                if (!Expression.TryParse(Stack, out operand)) {
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

            return result;
        }
    }
    
    /// <summary>
    /// Node for storing binary operation and its operands.
    /// </summary>
    class BinaryOperator : Expression {
        public Expression FirstOperand { get; set; }
        public Expression SecondOperand { get; set; }
        public Operation Operation { get; set; }

        public BinaryOperator(Expression FirstOperand, Expression SecondOperand, Operation Operation) {
            this.FirstOperand = FirstOperand;
            this.SecondOperand = SecondOperand;
            this.Operation = Operation;
        }

        public static bool TryParse(string RawData, out Operation operation) {
            operation = default(Operation);
            //TODO: finish BinaryOperator.TryParse()
            return false;
        }
    }

    /// <summary>
    /// Node for storing unary operation and its operand.
    /// </summary>
    class UnaryOperator : Expression {
        public Expression Operand { get; set; }
        public Operation Operation { get; set; }

        public UnaryOperator(Expression Operand, Operation Operation) {
            this.Operand = Operand;
            this.Operation = Operation;
        }
        public static bool TryParse(string RawData, out Operation operation) {
            operation = default(Operation);
            //TODO: finish UnaryOperator.TryParse()
            return false;
        }
    }

    /// <summary>
    /// Node for storing int value. (Leaf of expression tree.)
    /// </summary>
    class Number : Expression {
        public int Value { get; set; }

        public Number(int Value) {
            this.Value = Value;
        }

        public static bool TryParse(string RawData, out int value) {
            value = default(int);

            if (!int.TryParse(RawData, out value)) {
                return false;
            }

            return true;
        }
    }

    internal enum Operation { 
        Addition,
        Subtraction,
        Multiplication,
        Division,
        CastMinus
    }
}
