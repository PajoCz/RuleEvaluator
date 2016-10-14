using System.Configuration;
using NUnit.Framework;

namespace RuleEvaluator.Repository.Database.Test
{
    [TestFixture]
    public class RuleItemRepositoryTest
    {
        [Test]
        public void IntegrateTest_RepoLoad_FindOneRuleItemAndCheckHisFilterValue()
        {
            var repo = new RuleItemsRepository(ConfigurationManager.AppSettings.Get("ConnectionString"), "Ciselnik.p_GetSchemaColBySchemaKod", "Ciselnik.p_GetTranslatorDataBySchemaKod");
            var items = repo.Load("OdhadBodu");
            var found = items.Find("A", "B", "C", "7BN Perspektiva Důchod", 15);
            var outputValue = found.Output(0).FilterValue;
            Assert.AreEqual("C2/240", outputValue);
        }
    }
}
