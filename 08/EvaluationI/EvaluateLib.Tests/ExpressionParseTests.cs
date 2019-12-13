using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EvaluateLib;

namespace EvaluateLib.Tests {
    [TestClass]
    public class ExpressionParseTests {
        [TestMethod]
        public void SimpleInputCreatesTree() {
            Expression expression = new BinaryOperator(
                new UnaryOperator(new Number(1), Operation.CastMinus),
                new Number(3),
                Operation.Addition);
        }
    }
}
