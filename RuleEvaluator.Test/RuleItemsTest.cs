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
                new RuleItem(".*", ".*", ".*", "MyString", "1[0-4]", new Cell("ReturnValue1", CellInputOutputType.Output)),
                new RuleItem(".*", ".*", ".*", "MyString", "1[5-9]|2[0-4]", new Cell("ReturnValue2", CellInputOutputType.Output)),
            });
            Assert.AreEqual("ReturnValue2" ,items.Find("Anything", "Anything2", "Anything3", "MyString", "15").Output(0).FilterValue);
        }

        [Test]
        public void IntegrityTest_Find_OneFromMoreRuleItems_CellValidateDecimal_ReturnsCorrectOutputValue()
        {
            RuleItems items = new RuleItems(new List<RuleItem>()
            {
                new RuleItem(".*", ".*", ".*", "MyString", new Cell(new CellValidateFilterDecimal(10, true, 15, false), new CellValidateModuleDecimal()), new Cell("ReturnValue1", CellInputOutputType.Output)),
                new RuleItem(".*", ".*", ".*", "MyString", new Cell(new CellValidateFilterDecimal(15, true, 24, false), new CellValidateModuleDecimal()), new Cell("ReturnValue2", CellInputOutputType.Output)),
            });
            Assert.AreEqual("ReturnValue2", items.Find("Anything", "Anything2", "Anything3", "MyString", 20m).Output(0).FilterValue);
        }

        [Test]
        public void IntegrityTest_Find_OneFromMoreRuleItems_CellValidateDecimalByDetector_ReturnsCorrectOutputValue()
        {
            RuleItems items = new RuleItems(new List<RuleItem>()
            {
                new RuleItem(".*", ".*", ".*", "MyString", new CellValidateFilterDecimal(10, true, 15, false), new Cell("ReturnValue1", CellInputOutputType.Output)),
                new RuleItem(".*", ".*", ".*", "MyString", new CellValidateFilterDecimal(15, true, 24, false), new Cell("ReturnValue2", CellInputOutputType.Output)),
            });
            Assert.AreEqual("ReturnValue2", items.Find("Anything", "Anything2", "Anything3", "MyString", 20m).Output(0).FilterValue);
        }
    }
}
