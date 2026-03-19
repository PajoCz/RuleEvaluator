using NUnit.Framework;
using System;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class RuleItemTest
    {
        private static ICellFactory CreateFactory() => new DefaultCellFactory();

        [Test]
        public void Ctor_InputNotCellObject_AllCellAsInput()
        {
            var cf = CreateFactory();
            var ri = new RuleItem(cf, "text", 1);
            ri.Cells.ForEach(c => Assert.That(c.InputOutputType, Is.EqualTo(CellInputOutputType.Input)));
        }

        [Test]
        public void Ctor_InputCellObject_CanSetCellOutput()
        {
            var cf = CreateFactory();
            var ri = new RuleItem(cf, "text", cf.CreateCell("output", CellInputOutputType.Output));
            Assert.That(ri.Cells[1].InputOutputType, Is.EqualTo(CellInputOutputType.Output));
        }

        [Test]
        public void ValidateInput_OneInputOneOutput_ReturnsTrue()
        {
            var cf = CreateFactory();
            string input = "input";
            string output = "output";
            var ri = new RuleItem(cf, input, cf.CreateCell(output, CellInputOutputType.Output));
            Assert.That(ri.ValidateInput(RuleItemsName, input), Is.True);
        }

        [Test]
        public void ValidateInput_OneInputOneOutput_ReturnsFalse()
        {
            var cf = CreateFactory();
            string input = "text";
            string output = "output";
            var ri = new RuleItem(cf, input, cf.CreateCell(output, CellInputOutputType.Output));
            Assert.That(ri.ValidateInput(RuleItemsName, input + "changed"), Is.False);
        }

        [Test]
        public void ValidateInput_TwoInputsOneOutput_ReturnsTrue()
        {
            var cf = CreateFactory();
            string input1 = "text1";
            string input2 = "text2";
            string output = "output";
            var ri = new RuleItem(cf, input1, input2, cf.CreateCell(output, CellInputOutputType.Output));
            Assert.That(ri.ValidateInput(RuleItemsName, input1, input2), Is.True);
        }

        [Test]
        public void ValidateInput_TwoInputsOneOutput_OneInputIsCorrectAndOtherIncorrect_ValidateByAndOperatorReturnsFalse()
        {
            var cf = CreateFactory();
            string input1 = "text1";
            string input2 = "text2";
            string output = "output";
            var ri = new RuleItem(cf, input1, input2, cf.CreateCell(output, CellInputOutputType.Output));
            Assert.That(ri.ValidateInput(RuleItemsName, input1, input2 + "changed"), Is.False);
        }

        [Test]
        public void ValidateInput_ValidateInputLessParameters_ThrowsException()
        {
            string cellInput1 = "input1";
            int cellInput2 = 1;
            var cf = CreateFactory();
            var ri = new RuleItem(cf, cellInput1, cellInput2);
            Assert.Throws<InputParameterCountMismatchException>(() => ri.ValidateInput(RuleItemsName, cellInput1));
        }

        [Test]
        public void ValidateInput_ValidateInputMoreParameters_ThrowsException()
        {
            string cellInput1 = "input1";
            int cellInput2 = 1;
            var cf = CreateFactory();
            var ri = new RuleItem(cf, cellInput1);
            Assert.Throws<InputParameterCountMismatchException>(() => ri.ValidateInput(RuleItemsName, cellInput1, cellInput2));
        }

        const string RuleItemsName = "RuleItemsName";

        [Test]
        public void ValidateInput_IgnoreRuleItemWithCellOutput()
        {
            string cellInput1 = "input1";
            int cellInput2 = 1;
            var cf = CreateFactory();
            var ri = new RuleItem(cf, cellInput1, cf.CreateCell("output", CellInputOutputType.Output), cellInput2);
            var validated = ri.ValidateInput(RuleItemsName, cellInput1, cellInput2);
            Assert.That(validated, Is.True);
        }

        [Test]
        public void ValidateInput_ExpectedOneParameterAndValidatedWithoutAnyParametr_ThrowsException()
        {
            string cellInput1 = "input1";
            var cf = CreateFactory();
            var ri = new RuleItem(cf, cellInput1);
            Assert.Throws<InputParameterCountMismatchException>(() => ri.ValidateInput("test"));
        }
    }
}
