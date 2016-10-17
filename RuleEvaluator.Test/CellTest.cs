using Castle.Windsor;
using Castle.Windsor.Installer;
using NUnit.Framework;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class CellTest
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
        public void Ctor_SetFilterValueInCtor_FilterValueIsSet()
        {
            string input = "Text";
            var cell = new CellFactory(_WindsorContainer).CreateCell(input);
            Assert.AreEqual(input, cell.FilterValue);
        }

        [Test]
        public void Ctor_SetFilterValueWithoutInputOutputType_ReturnsCellWithInputType()
        {
            string input = "Text";
            var cell = new CellFactory(_WindsorContainer).CreateCell(input);
            Assert.AreEqual(CellInputOutputType.Input, cell.InputOutputType);
        }

        [Test]
        public void Ctor_SetFilterValueAndOutputType_ReturnsCellWithOutputType()
        {
            string input = "Text";
            var cell = new CellFactory(_WindsorContainer).CreateCell(input, CellInputOutputType.Output);
            Assert.AreEqual(CellInputOutputType.Output, cell.InputOutputType);
        }

        [Test]
        public void Validate_StringOriginal_ReturnTrue()
        {
            string input = "Text";
            var cell = new CellFactory(_WindsorContainer).CreateCell(input);
            Assert.IsTrue(cell.Validate(input));
        }

        [Test]
        public void Validate_StringChanged_ReturnFalse()
        {
            string input = "Text";
            var cell = new CellFactory(_WindsorContainer).CreateCell(input);
            Assert.IsFalse(cell.Validate(input + "Changed"));
        }

        [Test]
        public void Validate_IntOriginal_ReturnTrue()
        {
            int input = 10;
            var cell = new CellFactory(_WindsorContainer).CreateCell(input);
            Assert.IsTrue(cell.Validate(input));
        }

        [Test]
        public void Validate_IntIncremented_ReturnFalse()
        {
            int input = 10;
            var cell = new CellFactory(_WindsorContainer).CreateCell(input);
            Assert.IsFalse(cell.Validate(++input));
        }










        //[Test]
        //public void Validate_NotSetValidateModule_DefaultSetCellValidateRegex()
        //{
        //    Cell cell = new Cell("[1-2]");
        //    Assert.AreEqual(typeof(CellValidateModuleRegex), cell.CellValidateModuleModule.GetType());
        //}


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
