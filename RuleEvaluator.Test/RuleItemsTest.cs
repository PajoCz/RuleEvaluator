using System.Collections.Generic;
using NUnit.Framework;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class RuleItemsTest
    {
        [Test]
        public void IntegrityTest_Find_OneFromMoreRuleItems_ReturnsCorrectOutputValue()
        {
            RuleItems items = new RuleItems(new List<RuleItem>()
            {
                new RuleItem(".*", ".*", ".*", "7BN Perspektiva Důchod", "1[0-4]", new Cell("C2/400", CellInputOutputType.Output)),
                new RuleItem(".*", ".*", ".*", "7BN Perspektiva Důchod", "1[5-9]|2[0-4]", new Cell("C2/240", CellInputOutputType.Output)),
            });
            Assert.AreEqual("C2/240" ,items.Find("Cokoliv", "Cokoliv2", "Cokoliv3", "7BN Perspektiva Důchod", "15").Output(0).FilterValue);
        }

        [Test]
        public void IntegrityTest_Find_OneFromMoreRuleItems_CellValidateDecimal_ReturnsCorrectOutputValue()
        {
            RuleItems items = new RuleItems(new List<RuleItem>()
            {
                new RuleItem(".*", ".*", ".*", "7BN Perspektiva Důchod", new Cell(new CellValidateFilterDecimal(10, true, 15, false), new CellValidateModuleDecimal()), new Cell("C2/400", CellInputOutputType.Output)),
                new RuleItem(".*", ".*", ".*", "7BN Perspektiva Důchod", new Cell(new CellValidateFilterDecimal(15, true, 24, false), new CellValidateModuleDecimal()), new Cell("C2/240", CellInputOutputType.Output)),
            });
            Assert.AreEqual("C2/240", items.Find("Cokoliv", "Cokoliv2", "Cokoliv3", "7BN Perspektiva Důchod", 20m).Output(0).FilterValue);
        }

        [Test]
        public void IntegrityTest_Find_OneFromMoreRuleItems_CellValidateDecimalWithoutExplicitSpecifyModule_ReturnsCorrectOutputValue()
        {
            RuleItems items = new RuleItems(new List<RuleItem>()
            {
                new RuleItem(".*", ".*", ".*", "7BN Perspektiva Důchod", Cell.CreateByFilterModule(new CellValidateFilterDecimal(10, true, 15, false)), new Cell("C2/400", CellInputOutputType.Output)),
                new RuleItem(".*", ".*", ".*", "7BN Perspektiva Důchod", Cell.CreateByFilterModule(new CellValidateFilterDecimal(15, true, 24, false)), new Cell("C2/240", CellInputOutputType.Output)),
            });
            Assert.AreEqual("C2/240", items.Find("Cokoliv", "Cokoliv2", "Cokoliv3", "7BN Perspektiva Důchod", 20m).Output(0).FilterValue);
        }
    }
}
