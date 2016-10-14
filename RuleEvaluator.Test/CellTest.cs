using System;
using NUnit.Framework;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class CellTest
    {
        [Test]
        public void Ctor_SetFilterValueInCtor_FilterValueIsSet()
        {
            string input = "Text";
            Cell cell = new Cell(input);
            Assert.AreEqual(input, cell.FilterValue);
        }

        [Test]
        public void Ctor_SetFilterValueWithoutInputOutputType_ReturnsCellWithInputType()
        {
            string input = "Text";
            Cell cell = new Cell(input);
            Assert.AreEqual(CellInputOutputType.Input, cell.InputOutputType);
        }

        [Test]
        public void Ctor_SetFilterValueAndOutputType_ReturnsCellWithOutputType()
        {
            string input = "Text";
            Cell cell = new Cell(input, CellInputOutputType.Output);
            Assert.AreEqual(CellInputOutputType.Output, cell.InputOutputType);
        }

        [Test]
        public void Ctor_SetNullAsFilterValue_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var cell = new Cell(null);
            });
        }

        [Test]
        public void Validate_StringOriginal_ReturnTrue()
        {
            string input = "Text";
            Cell cell = new Cell(input);
            Assert.IsTrue(cell.Validate(input));
        }

        [Test]
        public void Validate_StringChanged_ReturnFalse()
        {
            string input = "Text";
            Cell cell = new Cell(input);
            Assert.IsFalse(cell.Validate(input + "Changed"));
        }

        [Test]
        public void Validate_IntOriginal_ReturnTrue()
        {
            int input = 10;
            Cell cell = new Cell(input);
            Assert.IsTrue(cell.Validate(input));
        }

        [Test]
        public void Validate_IntIncremented_ReturnFalse()
        {
            int input = 10;
            Cell cell = new Cell(input);
            Assert.IsFalse(cell.Validate(++input));
        }

        [Test]
        public void Validate_NotSetValidateModule_DefaultSetCellValidateRegex()
        {
            Cell cell = new Cell("[1-2]");
            Assert.AreEqual(typeof(CellValidateModuleRegex), cell.CellValidateModuleModule.GetType());
        }


        //[Test]
        //public void Validate_DefaultStringRegex_ReturnTrue()
        //{
        //    Cell cell = new Cell("1[1-9]");
        //    Assert.IsTrue(cell.Validate("11"));
        //    Assert.IsTrue(cell.Validate("15"));
        //    Assert.IsTrue(cell.Validate("19"));
        //}

        //[Test]
        //public void Validate_DefaultStringRegex_ReturnFalse()
        //{
        //    Cell cell = new Cell("1[1-9]");
        //    Assert.IsFalse(cell.Validate("21"));
        //    Assert.IsFalse(cell.Validate("25"));
        //    Assert.IsFalse(cell.Validate("111"));
        //}
    }
}
