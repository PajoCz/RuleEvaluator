using NUnit.Framework;
using System;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class RuleItemTest
    {
        [Test]
        public void Ctor_InputNotCellObject_AllCellAsInput()
        {
            var ri = new RuleItem("text", 1);
            ri.Cells.ForEach(c => Assert.AreEqual(CellInputOutputType.Input, c.InputOutputType));
        }

        [Test]
        public void Ctor_InputCellObject_CanSetCellOutput()
        {
            var ri = new RuleItem("text", new Cell("vystup", CellInputOutputType.Output));
            Assert.AreEqual(CellInputOutputType.Output, ri.Cells[1].InputOutputType);
        }

        [Test]
        public void ValidateInput_ValidateInputLessParameters_ThrownException()
        {
            string cellInput1 = "input1";
            int cellInput2 = 1;
            var ri = new RuleItem(cellInput1, cellInput2);
            Assert.Throws<ArgumentOutOfRangeException>(() => ri.ValidateInput(cellInput1));
        }

        [Test]
        public void ValidateInput_ValidateInputMoreParameters_ThrownException()
        {
            string cellInput1 = "input1";
            int cellInput2 = 1;
            var ri = new RuleItem(cellInput1);
            Assert.Throws<ArgumentOutOfRangeException>(() => ri.ValidateInput(cellInput1, cellInput2));
        }

        [Test]
        public void ValidateInput_IgnoreRuleItemWithCellOutput()
        {
            string cellInput1 = "input1";
            int cellInput2 = 1;
            var ri = new RuleItem(cellInput1, new Cell("vystup", CellInputOutputType.Output), cellInput2);
            Assert.IsTrue(ri.ValidateInput(cellInput1, cellInput2));
        }
    }
}
