using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hervis.Excell;

namespace Excell.Tests {
    [TestClass]
    public class EquationParseTests {
        private const string WithoutEqualsSymbol = "A1+B2";
        private const string EmptyEquation = "=";
        private const string MissingFirstOperand = "=A1+";
        private const string MissingSecondOperand = "=-A1";
        private const string MissingOperation = "=A1A2";
        private const string IncompleteOperand = "=A+B3";
        private const string GarbageInOperand = "=Aa43+B***";
        private const string WrongOperand = "=ahoj+B3";
        private const string WrongNumberOfOperands = "=result+tramvaj-autobus";
        private const string ShortAddress = "=A10+W4";
        private const string LongAddress = "=ABC1769874983+ZZZZK1256376569";

        [TestMethod]
        public void MissingEqualsSignSetsInvalid() {
            CellType correctAnswer = CellType.Invalid;
            CellType myAnswer = default(CellType);
            Equation myEquation = default(Equation);

            Equation.TryParse(WithoutEqualsSymbol, out myEquation, out myAnswer);
            Assert.IsTrue(Utilities.AreEqual(correctAnswer, myAnswer));
        }
        [TestMethod]
        public void EmptyEquationSetsMissing() {
            CellType correctAnswer = CellType.MissingOperation;
            CellType myAnswer = default(CellType);
            Equation myEquation = default(Equation);

            Equation.TryParse(EmptyEquation, out myEquation, out myAnswer);
            Assert.IsTrue(Utilities.AreEqual(correctAnswer, myAnswer));
        }

        [TestMethod]
        public void MissingFirstOperandSetsFormula() {
            CellType correctAnswer = CellType.Formula;
            CellType myAnswer = default(CellType);
            Equation myEquation = default(Equation);

            Equation.TryParse(MissingFirstOperand, out myEquation, out myAnswer);
            Assert.IsTrue(Utilities.AreEqual(correctAnswer, myAnswer));
        }

        [TestMethod]
        public void MissingSecondOperandSetsFormula() {
            CellType correctAnswer = CellType.Formula;
            CellType myAnswer = default(CellType);
            Equation myEquation = default(Equation);

            Equation.TryParse(MissingSecondOperand, out myEquation, out myAnswer);
            Assert.IsTrue(Utilities.AreEqual(correctAnswer, myAnswer));
        }

        [TestMethod]
        public void MissingOperationSetsMissing() {
            CellType correctAnswer = CellType.MissingOperation;
            CellType myAnswer = default(CellType);
            Equation myEquation = default(Equation);

            Equation.TryParse(MissingOperation, out myEquation, out myAnswer);
            Assert.IsTrue(Utilities.AreEqual(correctAnswer, myAnswer));
        }

        [TestMethod]
        public void IncompleteOperandSetsFormula() {
            CellType correctAnswer = CellType.Formula;
            CellType myAnswer = default(CellType);
            Equation myEquation = default(Equation);

            Equation.TryParse(IncompleteOperand, out myEquation, out myAnswer);
            Assert.IsTrue(Utilities.AreEqual(correctAnswer, myAnswer));
        }

        [TestMethod]
        public void GarbageInOperandSetsFormula() {
            CellType correctAnswer = CellType.Formula;
            CellType myAnswer = default(CellType);
            Equation myEquation = default(Equation);

            Equation.TryParse(GarbageInOperand, out myEquation, out myAnswer);
            Assert.IsTrue(Utilities.AreEqual(correctAnswer, myAnswer));
        }

        [TestMethod]
        public void WrongOperandSetsFormula() {
            CellType correctAnswer = CellType.Formula;
            CellType myAnswer = default(CellType);
            Equation myEquation = default(Equation);

            Equation.TryParse(WrongOperand, out myEquation, out myAnswer);
            Assert.IsTrue(Utilities.AreEqual(correctAnswer, myAnswer));
        }

        [TestMethod]
        public void WrongNumberOfOperandsSetsFormula() {
            CellType correctAnswer = CellType.Formula;
            CellType myAnswer = default(CellType);
            Equation myEquation = default(Equation);

            Equation.TryParse(WrongNumberOfOperands, out myEquation, out myAnswer);
            Assert.IsTrue(Utilities.AreEqual(correctAnswer, myAnswer));
        }

        [TestMethod]
        public void CorrectTranslationOfAdresses() {
            CellType myAnswer = default(CellType);
            Equation myEquation = default(Equation);
            Coords[] correctCoords = new Coords[ 4 ];
            Coords[] myCoords = new Coords[ 4 ];

            correctCoords[ 0 ] = new Coords(9, 0);
            correctCoords[ 1 ] = new Coords(3, 22);
            correctCoords[ 2 ] = new Coords(1769874982, 730);
            correctCoords[ 3 ] = new Coords(1256376568, 12356614);

            Equation.TryParse(ShortAddress, out myEquation, out myAnswer);
            myCoords[ 0 ] = myEquation.FirstOperand;
            myCoords[ 1 ] = myEquation.SecondOperand;

            Equation.TryParse(LongAddress, out myEquation, out myAnswer);
            myCoords[ 2 ] = myEquation.FirstOperand;
            myCoords[ 3 ] = myEquation.SecondOperand;

            Assert.IsTrue(Utilities.AreEqual(correctCoords, myCoords));
        }
    }
}
