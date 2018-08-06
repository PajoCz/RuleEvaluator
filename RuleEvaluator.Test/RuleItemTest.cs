using NUnit.Framework;
using System;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class RuleItemTest
    {
        private static IWindsorContainer _WindsorContainer
        {
            get
            {
                IWindsorContainer container = new WindsorContainer();
                container.Install(FromAssembly.InThisApplication());
                return container;
            }
        }

        [Test]
        public void Ctor_InputNotCellObject_AllCellAsInput()
        {
            //Arrange
            var cf = _WindsorContainer.Resolve<ICellFactory>();

            //Act
            var ri = new RuleItem(cf, "text", 1);

            //Assert
            ri.Cells.ForEach(c => Assert.AreEqual(CellInputOutputType.Input, c.InputOutputType));
        }

        [Test]
        public void Ctor_InputCellObject_CanSetCellOutput()
        {
            //Arrange
            var cf = _WindsorContainer.Resolve<ICellFactory>();

            //Act
            var ri = new RuleItem(cf, "text", cf.CreateCell("output", CellInputOutputType.Output));

            //Assert
            Assert.AreEqual(CellInputOutputType.Output, ri.Cells[1].InputOutputType);
        }

        [Test]
        public void ValidateInput_OneInputOneOutput_ReturnsTrue()
        {
            //Arrange
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            string input = "input";
            string output = "output";

            //Act
            var ri = new RuleItem(cf, input, cf.CreateCell(output, CellInputOutputType.Output));

            //Assert
            Assert.IsTrue(ri.ValidateInput(_RuleItemsName, input));
        }

        [Test]
        public void ValidateInput_OneInputOneOutput_ReturnsFalse()
        {
            //Arrange
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            string input = "text";
            string output = "output";

            //Act
            var ri = new RuleItem(cf, input, cf.CreateCell(output, CellInputOutputType.Output));

            //Assert
            Assert.IsFalse(ri.ValidateInput(_RuleItemsName, input + "changed"));
        }

        [Test]
        public void ValidateInput_TwoInputsOneOutput_ReturnsTrue()
        {
            //Arrange
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            string input1 = "text1";
            string input2 = "text2";
            string output = "output";

            //Act
            var ri = new RuleItem(cf, input1, input2, cf.CreateCell(output, CellInputOutputType.Output));

            //Assert
            Assert.IsTrue(ri.ValidateInput(_RuleItemsName, input1, input2));
        }

        [Test]
        public void ValidateInput_TwoInputsOneOutput_OneInputIsCorrectAndOtherIncorrect_ValidateByAndOperatorReturnsFalse()
        {
            //Arrange
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            string input1 = "text1";
            string input2 = "text2";
            string output = "output";

            //Act
            var ri = new RuleItem(cf, input1, input2, cf.CreateCell(output, CellInputOutputType.Output));

            //Assert
            Assert.IsFalse(ri.ValidateInput(_RuleItemsName, input1, input2 + "changed"));
        }

        [Test]
        public void ValidateInput_ValidateInputLessParameters_ThrowsException()
        {
            //Arrange
            string cellInput1 = "input1";
            int cellInput2 = 1;
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            var ri = new RuleItem(cf, cellInput1, cellInput2);

            //Act with Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => ri.ValidateInput(_RuleItemsName, cellInput1));
        }

        [Test]
        public void ValidateInput_ValidateInputMoreParameters_ThrowsException()
        {
            //Arrange
            string cellInput1 = "input1";
            int cellInput2 = 1;
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            var ri = new RuleItem(cf, cellInput1);

            //Act with Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => ri.ValidateInput(_RuleItemsName, cellInput1, cellInput2));
        }

        const string _RuleItemsName = "RuleItemsName";

        [Test]
        public void ValidateInput_IgnoreRuleItemWithCellOutput()
        {
            //Arrange
            string cellInput1 = "input1";
            int cellInput2 = 1;
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            var ri = new RuleItem(cf, cellInput1, cf.CreateCell("output", CellInputOutputType.Output), cellInput2);

            //Act
            var validated = ri.ValidateInput(_RuleItemsName, cellInput1, cellInput2);

            //Assert
            Assert.IsTrue(validated);
        }

        [Test]
        public void ValidateInput_ExpectedOneParameterAndValidatedWithoutAnyParametr_ThrowsException()
        {
            //Arrange
            string cellInput1 = "input1";
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            var ri = new RuleItem(cf, cellInput1);

            //Act with Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => ri.ValidateInput("test"));
        }
    }
}
