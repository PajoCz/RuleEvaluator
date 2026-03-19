using System;
using NUnit.Framework;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class CellTest
    {
        private static ICellFactory CreateFactory() => new DefaultCellFactory();

        [Test]
        public void Ctor_SetFilterValueInCtor_FilterValueIsSet1()
        {
            string input = "Text";
            var cf = CreateFactory();
            var cell = cf.CreateCell(input);
            Assert.That(cell.FilterValue, Is.EqualTo(input));
        }

        [Test]
        public void Ctor_SetFilterValueWithoutInputOutputType_ReturnsCellWithInputType()
        {
            string input = "Text";
            var cf = CreateFactory();
            var cell = cf.CreateCell(input);
            Assert.That(cell.InputOutputType, Is.EqualTo(CellInputOutputType.Input));
        }

        [Test]
        public void Ctor_SetFilterValueAndOutputType_ReturnsCellWithOutputType()
        {
            string input = "Text";
            var cf = CreateFactory();
            var cell = cf.CreateCell(input, CellInputOutputType.Output);
            Assert.That(cell.InputOutputType, Is.EqualTo(CellInputOutputType.Output));
        }

        [Test]
        public void Validate_StringOriginal_ReturnTrue()
        {
            string input = "Text";
            var cf = CreateFactory();
            var cell = cf.CreateCell(input);
            Assert.That(cell.Validate(input), Is.True);
        }

        [Test]
        public void Validate_StringChanged_ReturnFalse()
        {
            string input = "Text";
            var cf = CreateFactory();
            var cell = cf.CreateCell(input);
            Assert.That(cell.Validate(input + "Changed"), Is.False);
        }

        [Test]
        public void Validate_IntOriginal_ReturnTrue()
        {
            int input = 10;
            var cf = CreateFactory();
            var cell = cf.CreateCell(input);
            Assert.That(cell.Validate(input), Is.True);
        }

        [Test]
        public void Validate_IntIncremented_ReturnFalse()
        {
            int input = 10;
            var cf = CreateFactory();
            var cell = cf.CreateCell(input);
            Assert.That(cell.Validate(++input), Is.False);
        }

        [Test]
        public void Validate_AllCellsValidateModuleInChainOfResponsibilityReturnsNull_ThrowsMatcherException()
        {
            int input = 10;
            var factory = new DefaultCellFactory(new CellValidateModuleUnknown());
            var cell = factory.CreateCell(input);
            var exc = Assert.Throws<MatcherException>(() => cell.Validate(input));
            Assert.That(exc!.Message, Is.EqualTo("Unknown validate result from ICellValidateModule instances"));
        }

        private class CellValidateModuleUnknown : ICellValidateModule
        {
            public bool? Validate(object p_CellFilter, object p_ValueDataForValidating)
            {
                return null;
            }
        }
    }
}

