using System;
using System.Configuration;
using System.Diagnostics;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using NUnit.Framework;
using RuleEvaluator.Repository.Contract;

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

        private static DatabaseType GetDatabaseType()
        {
            var dbTypeString = ConfigurationManager.AppSettings.Get("ConnectionStringType");
            if (string.IsNullOrEmpty(dbTypeString))
                return DatabaseType.MSSQL;

            if (Enum.TryParse<DatabaseType>(dbTypeString, true, out var dbType))
                return dbType;

            return DatabaseType.MSSQL;
        }

        [Test]
        public void IntegrityTest_RepoLoad_FindOneRuleItemAndCheckFilterValue_LocalDatabase()
        {
            //Arrange
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            var cache = _WindsorContainer.Resolve<ICacheWrapper>();
            var dbType = GetDatabaseType();

            //Act
            var repo = new RuleItemsRepository(cf, cache, ConfigurationManager.AppSettings.Get("ConnectionString"), "Rules.p_GetSchemaColBySchemaKey", "Rules.p_GetTranslatorDataBySchemaKey", TimeSpan.FromMinutes(10), dbType);
            //var repo = new RuleItemsRepository(cf, cache, ConfigurationManager.AppSettings.Get("ConnectionString"), "rules.p_getschemacol_byschemakey", "rules.p_gettranslatordata_byschemakey", TimeSpan.FromMinutes(10), dbType);
            var items = repo.Load("Translator1Schema1");
            var found = items.Find("a", "0");
            var outputValue = found.Output(0).FilterValue;

            //Assert
            Assert.AreEqual("Result1", outputValue);
            Assert.AreEqual(1, found.PrimaryKey?.FilterValue, "PrimaryKey of found item must be filled when readed from Database");
        }

        [Test]
        public void IntegrityTest_RepoLoad_FindOneRuleItemAndCheckFilterValue()
        {
            //Arrange
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            var cache = _WindsorContainer.Resolve<ICacheWrapper>();
            var dbType = GetDatabaseType();

            //Act
            var repo = new RuleItemsRepository(cf, cache, ConfigurationManager.AppSettings.Get("ConnectionString"), "Ciselnik.p_GetSchemaColBySchemaKod", "Ciselnik.p_GetTranslatorDataBySchemaKod", TimeSpan.FromMinutes(10), dbType);
            var items = repo.Load("OdhadBodu");
            var found = items.Find("A", "B", "C", "7BN Perspektiva Důchod", 15);
            var outputValue = found.Output(0).FilterValue;

            //Assert
            Assert.AreEqual("C2/240*0.7", outputValue);
            Assert.AreEqual(89, found.PrimaryKey?.FilterValue, "PrimaryKey of found item must be filled when readed from Database");
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
