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
            var ri = new RuleItem(cf, "text", cf.CreateCell("vystup", CellInputOutputType.Output));

            //Assert
            Assert.AreEqual(CellInputOutputType.Output, ri.Cells[1].InputOutputType);
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
            Assert.Throws<ArgumentOutOfRangeException>(() => ri.ValidateInput(cellInput1));
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
            Assert.Throws<ArgumentOutOfRangeException>(() => ri.ValidateInput(cellInput1, cellInput2));
        }

        [Test]
        public void ValidateInput_IgnoreRuleItemWithCellOutput()
        {
            //Arrange
            string cellInput1 = "input1";
            int cellInput2 = 1;
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            var ri = new RuleItem(cf, cellInput1, cf.CreateCell("vystup", CellInputOutputType.Output), cellInput2);

            //Act
            var validated = ri.ValidateInput(cellInput1, cellInput2);

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
            Assert.Throws<ArgumentOutOfRangeException>(() => ri.ValidateInput());
        }
    }
}
