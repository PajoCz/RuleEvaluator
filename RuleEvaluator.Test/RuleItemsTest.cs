using NUnit.Framework;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class RuleItemsTest
    {
        private static ICellFactory CreateFactory() => new DefaultCellFactory();

        [Test]
        public void IntegrityTest_Find_OneFromMoreRuleItems_ReturnsCorrectOutputValue()
        {
            var cf = CreateFactory();
            RuleItems items = new RuleItems(cf);
            items.AddRuleItem(".*", ".*", ".*", "MyString", "1[0-4]", cf.CreateCell("ReturnValue1", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", "1[5-9]|2[0-4]", cf.CreateCell("ReturnValue2", CellInputOutputType.Output));

            var found = items.Find("Anything", "Anything2", "Anything3", "MyString", "15")!.Output(0).FilterValue;
            Assert.That(found, Is.EqualTo("ReturnValue2"));
        }

        [Test]
        public void IntegrityTest_Find_OneFromMoreRuleItems_CellValidateDecimalByDetector_ReturnsCorrectOutputValue()
        {
            var cf = CreateFactory();
            RuleItems items = new RuleItems(cf);
            items.AddRuleItem(".*", ".*", ".*", "MyString", cf.CreateCell(new CellValidateFilterDecimalInterval(10, true, 15, false)),
                cf.CreateCell("ReturnValue1", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", cf.CreateCell(new CellValidateFilterDecimalInterval(15, true, 24, true)),
                cf.CreateCell("ReturnValue2", CellInputOutputType.Output));

            var found = items.Find("Anything", "Anything2", "Anything3", "MyString", 20m)!.Output(0).FilterValue;
            Assert.That(found, Is.EqualTo("ReturnValue2"));
        }

        [Test]
        public void IntegrityTest_Find_OneFromMoreRuleItems_IntervalStringAsCellValidateFilterDecimal_ReturnsCorrectOutputValue()
        {
            var cf = CreateFactory();
            RuleItems items = new RuleItems(cf);
            items.AddRuleItem(".*", ".*", ".*", "MyString", "Interval<10;15)", cf.CreateCell("ReturnValue1", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", "INTERVAL<15;24>", cf.CreateCell("ReturnValue2", CellInputOutputType.Output));

            var found = items.Find("Anything", "Anything2", "Anything3", "MyString", 15m)!.Output(0).FilterValue;
            Assert.That(found, Is.EqualTo("ReturnValue2"));
        }

        [Test]
        public void IntegrityTest_FindAll()
        {
            var cf = CreateFactory();
            RuleItems items = new RuleItems(cf);
            items.AddRuleItem(".*", ".*", ".*", "MyString", "Interval<10;15>", cf.CreateCell("ReturnValue1", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", "INTERVAL<15;24>", cf.CreateCell("ReturnValue2", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", "INTERVAL(50;100)", cf.CreateCell("ReturnValue3", CellInputOutputType.Output));

            var count = items.FindAll("Anything", "Anything2", "Anything3", "MyString", 15m).Count;
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void IntegrityTest_Find_OutputAsRawString()
        {
            // Output can be a raw expression string (e.g., "C2/240") - engine should NOT evaluate it
            var cf = CreateFactory();
            RuleItems items = new RuleItems(cf);
            items.AddRuleItem(".*", ".*", ".*", "MyString", "INTERVAL<15;24>", cf.CreateCell("C2/240", CellInputOutputType.Output));

            var filterValue = items.Find("Anything", "Anything2", "Anything3", "MyString", 15m)!.Output(0).FilterValue;
            Assert.That(filterValue, Is.EqualTo("C2/240"));
        }

        [Test]
        public void Find_NoMatch_ReturnsNull()
        {
            var cf = CreateFactory();
            RuleItems items = new RuleItems(cf);
            items.AddRuleItem("Exact", cf.CreateCell("Output", CellInputOutputType.Output));

            var result = items.Find("NoMatch");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void FindAll_NoMatch_ReturnsEmptyList()
        {
            var cf = CreateFactory();
            RuleItems items = new RuleItems(cf);
            items.AddRuleItem("Exact", cf.CreateCell("Output", CellInputOutputType.Output));

            var result = items.FindAll("NoMatch");
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void PrimaryKey_Role_IsPreserved()
        {
            var cf = CreateFactory();
            RuleItems items = new RuleItems(cf);
            items.AddRuleItem(cf.CreateCell(42, CellInputOutputType.PrimaryKey), ".*", cf.CreateCell("Output", CellInputOutputType.Output));

            var found = items.Find("anything")!;
            Assert.That(found.PrimaryKey, Is.Not.Null);
            Assert.That(found.PrimaryKey!.FilterValue, Is.EqualTo(42));
        }
    }
}
