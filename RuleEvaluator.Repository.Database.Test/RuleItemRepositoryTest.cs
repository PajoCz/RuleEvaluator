using System.Configuration;
using System.Diagnostics;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using NUnit.Framework;

namespace RuleEvaluator.Repository.Database.Test
{
    [TestFixture]
    public class RuleItemRepositoryTest
    {
        private static IWindsorContainer _WindsorContainer
        {
            get
            {
                IWindsorContainer container = new WindsorContainer();
                container.Install(FromAssembly.InThisApplication());
                container.Register(Component.For<ICacheWrapper>().ImplementedBy<CacheWrapperMemory>().LifestyleTransient());
                return container;
            }
        }

        [Test]
        public void IntegrityTest_RepoLoad_FindOneRuleItemAndCheckFilterValue()
        {
            //Arrange
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            var cache = _WindsorContainer.Resolve<ICacheWrapper>();

            //Act
            var repo = new RuleItemsRepository(cf, cache, ConfigurationManager.AppSettings.Get("ConnectionString"), "Ciselnik.p_GetSchemaColBySchemaKod", "Ciselnik.p_GetTranslatorDataBySchemaKod");
            var items = repo.Load("OdhadBodu");
            var found = items.Find("A", "B", "C", "7BN Perspektiva Důchod", 15);
            var outputValue = found.Output(0).FilterValue;

            //Assert
            Assert.AreEqual("C2/240*0.7", outputValue);
        }

        //[Test]
        //public void PerformanceTest()
        //{
        //    //Arrange
        //    var cf = _WindsorContainer.Resolve<ICellFactory>();
        //    var cache = _WindsorContainer.Resolve<ICacheWrapper>();

        //    //Act
        //    var repo = new RuleItemsRepository(cf, cache, ConfigurationManager.AppSettings.Get("ConnectionString"), "Ciselnik.p_GetSchemaColBySchemaKod", "Ciselnik.p_GetTranslatorDataBySchemaKod");

        //    Stopwatch sw = Stopwatch.StartNew();
        //    var items = repo.Load("VisibilityField");
        //    var found = items.Find("Komoditni - Cil", "Cil", "Nazev", "10", "", "", "", "");
        //    var outputValue = found.Output(0).FilterValue;

        //    var items2 = repo.Load("MandatoryField");
        //    var found2 = items2.Find("Komoditni - Cil", "Cil", "Nazev", "10", "", "", "", "");
        //    var outputValue2 = found2.Output(0).FilterValue;
        //    sw.Stop();
        //    Debug.WriteLine($"Poprve {sw.Elapsed}");

        //    sw.Restart();
        //    var items3 = repo.Load("VisibilityField");
        //    var found3 = items3.Find("Komoditni - Cil", "Cil", "DatumPocatku", "10", "", "", "", "");
        //    var outputValue3 = found3.Output(0).FilterValue;

        //    var items4 = repo.Load("MandatoryField");
        //    var found4 = items4.Find("Komoditni - Cil", "Cil", "DatumPocatku", "10", "", "", "", "");
        //    var outputValue4 = found4.Output(0).FilterValue;
        //    Debug.WriteLine($"Podruhe {sw.Elapsed}");
        //    //Assert
        //    //Assert.AreEqual("C2/240*0.7", outputValue);
        //}
    }
}
