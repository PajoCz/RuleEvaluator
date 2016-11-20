using System;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
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
        public void Ctor_SetFilterValueInCtor_FilterValueIsSet1()
        {
            string input = "Text";
            var cell = _WindsorContainer.Resolve<ICellFactory>().CreateCell(input);
            Assert.AreEqual(input, cell.FilterValue);
        }

        [Test]
        public void Ctor_SetFilterValueWithoutInputOutputType_ReturnsCellWithInputType()
        {
            string input = "Text";
            var cell = _WindsorContainer.Resolve<ICellFactory>().CreateCell(input);
            Assert.AreEqual(CellInputOutputType.Input, cell.InputOutputType);
        }

        [Test]
        public void Ctor_SetFilterValueAndOutputType_ReturnsCellWithOutputType()
        {
            string input = "Text";
            var cell = _WindsorContainer.Resolve<ICellFactory>().CreateCell(input, CellInputOutputType.Output);
            Assert.AreEqual(CellInputOutputType.Output, cell.InputOutputType);
        }

        [Test]
        public void Validate_StringOriginal_ReturnTrue()
        {
            string input = "Text";
            var cell = _WindsorContainer.Resolve<ICellFactory>().CreateCell(input);
            Assert.IsTrue(cell.Validate(input));
        }

        [Test]
        public void Validate_StringChanged_ReturnFalse()
        {
            string input = "Text";
            var cell = _WindsorContainer.Resolve<ICellFactory>().CreateCell(input);
            Assert.IsFalse(cell.Validate(input + "Changed"));
        }

        [Test]
        public void Validate_IntOriginal_ReturnTrue()
        {
            int input = 10;
            var cell = _WindsorContainer.Resolve<ICellFactory>().CreateCell(input);
            Assert.IsTrue(cell.Validate(input));
        }

        [Test]
        public void Validate_IntIncremented_ReturnFalse()
        {
            int input = 10;
            var cell = _WindsorContainer.Resolve<ICellFactory>().CreateCell(input);
            Assert.IsFalse(cell.Validate(++input));
        }

        //[Test]
        //public void Validate_FilterValueIsNull_ThrowsException()
        //{
        //    var cell = _WindsorContainer.Resolve<ICellFactory>().CreateCell(null, CellInputOutputType.Input);
        //    Assert.Throws<ArgumentNullException>(() => cell.Validate(10));
        //}

        [Test]
        public void Validate_AllCellsValidateModuleInChainOfResponsibilityReturnsNull_ThrowsException()
        {
            IWindsorContainer container = new WindsorContainer();
            container.AddFacility<TypedFactoryFacility>();
            container.Register(Component.For<ICellFactory>().AsFactory());
            container.Register(Component.For<ICell>().ImplementedBy<Cell>().LifestyleTransient());
            container.Register(Component.For<ICellValidateModule>().ImplementedBy<CellValidateModuleUnknown>().LifestyleSingleton());

            int input = 10;
            var cell = container.Resolve<ICellFactory>().CreateCell(input);
            var exc = Assert.Throws<Exception>(() => cell.Validate(input));
            Assert.AreEqual("Uknown validate result from ICellValidateModule instances", exc.Message);
            //Assert.That(() => cell.Validate(input), Throws.TypeOf<Exception>().With.Message.EqualTo("Uknown validate result from ICellValidateModule instances"));
        }

        private class CellValidateModuleUnknown : ICellValidateModule
        {
            public bool? Validate(object p_CellFilter, object p_ValueDataForValidating)
            {
                return null;
            }
        }

        /// <summary>
        /// CellFactory only Resolve instance and never call Release.
        /// So check that container is not tracking instance. Cell doesn't implement IDisposable (windsor castle is tracking disposable transient objects)
        /// read this : http://tommarien.github.io/blog/2012/04/21/castle-windsor-avoid-memory-leaks-by-learning-the-underlying-mechanics/
        /// </summary>
        [Test]
        public void ContainerWithoutTracking_MeansTransientLifestyle()
        {
            int input = 10;
            var cell = _WindsorContainer.Resolve<ICellFactory>().CreateCell(input);
            Assert.IsFalse(_WindsorContainer.Kernel.ReleasePolicy.HasTrack(cell));
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
