using System;
using System.Configuration;
using NUnit.Framework;
using RuleEvaluator.Repository.Contract;

namespace RuleEvaluator.Repository.Database.Test
{
    [TestFixture]
    public class RuleItemRepositoryTest
    {
        private static ICellFactory CreateFactory() => new DefaultCellFactory();

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
            var cf = CreateFactory();
            var cache = new CacheWrapperMemory();
            var dbType = GetDatabaseType();

            var repo = new RuleItemsRepository(cf, cache, ConfigurationManager.AppSettings.Get("ConnectionString")!, "Rules.p_GetSchemaColBySchemaKey", "Rules.p_GetTranslatorDataBySchemaKey", TimeSpan.FromMinutes(10), dbType);
            var items = repo.Load("Translator1Schema1");
            var found = items.Find("a", "0");
            var outputValue = found!.Output(0).FilterValue;

            Assert.That(outputValue, Is.EqualTo("Result1"));
            Assert.That(found.PrimaryKey?.FilterValue, Is.EqualTo(1), "PrimaryKey of found item must be filled when readed from Database");
        }

        [Test]
        public void IntegrityTest_RepoLoad_FindOneRuleItemAndCheckFilterValue()
        {
            var cf = CreateFactory();
            var cache = new CacheWrapperMemory();
            var dbType = GetDatabaseType();

            var repo = new RuleItemsRepository(cf, cache, ConfigurationManager.AppSettings.Get("ConnectionString")!, "Ciselnik.p_GetSchemaColBySchemaKod", "Ciselnik.p_GetTranslatorDataBySchemaKod", TimeSpan.FromMinutes(10), dbType);
            var items = repo.Load("OdhadBodu");
            var found = items.Find("A", "B", "C", "7BN Perspektiva Důchod", 15);
            var outputValue = found!.Output(0).FilterValue;

            Assert.That(outputValue, Is.EqualTo("C2/240*0.7"));
            Assert.That(found.PrimaryKey?.FilterValue, Is.EqualTo(89), "PrimaryKey of found item must be filled when readed from Database");
        }
    }
}
