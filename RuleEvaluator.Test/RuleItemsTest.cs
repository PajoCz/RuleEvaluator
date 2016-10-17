using System;
using System.Diagnostics;
using Castle.Windsor;
using Castle.Windsor.Installer;
using NCalc;
using NUnit.Framework;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class RuleItemsTest
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
        public void IntegrityTest_Find_OneFromMoreRuleItems_ReturnsCorrectOutputValue()
        {
            RuleItems items = new RuleItems(_WindsorContainer);
            items.AddRuleItem(".*", ".*", ".*", "MyString", "1[0-4]", new CellFactory(_WindsorContainer).CreateCell("ReturnValue1", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", "1[5-9]|2[0-4]", new CellFactory(_WindsorContainer).CreateCell("ReturnValue2", CellInputOutputType.Output));
            Assert.AreEqual("ReturnValue2", items.Find("Anything", "Anything2", "Anything3", "MyString", "15").Output(0).FilterValue);
        }

        [Test]
        public void IntegrityTest_Find_OneFromMoreRuleItems_CellValidateDecimalByDetector_ReturnsCorrectOutputValue()
        {
            RuleItems items = new RuleItems(_WindsorContainer);
            items.AddRuleItem(".*", ".*", ".*", "MyString", new CellFactory(_WindsorContainer).CreateCell(new CellValidateFilterDecimalInterval(10, true, 15, false)),
                new CellFactory(_WindsorContainer).CreateCell("ReturnValue1", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", new CellFactory(_WindsorContainer).CreateCell(new CellValidateFilterDecimalInterval(15, true, 24, false)),
                new CellFactory(_WindsorContainer).CreateCell("ReturnValue2", CellInputOutputType.Output));
            Assert.AreEqual("ReturnValue2", items.Find("Anything", "Anything2", "Anything3", "MyString", 20m).Output(0).FilterValue);
        }

        [Test]
        public void IntegrityTest_Find_OneFromMoreRuleItems_IntervalStringAsCellValidateFilterDecimal_ReturnsCorrectOutputValue()
        {
            RuleItems items = new RuleItems(_WindsorContainer);
            items.AddRuleItem(".*", ".*", ".*", "MyString", "Interval<10;15)", new CellFactory(_WindsorContainer).CreateCell("ReturnValue1", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", "INTERVAL<15;24)", new CellFactory(_WindsorContainer).CreateCell("ReturnValue2", CellInputOutputType.Output));
            Assert.AreEqual("ReturnValue2", items.Find("Anything", "Anything2", "Anything3", "MyString", 15m).Output(0).FilterValue);
        }

        [Test]
        public void IntegrityTest_FindAll_Test1()
        {
            RuleItems items = new RuleItems(_WindsorContainer);
            items.AddRuleItem(".*", ".*", ".*", "MyString", "Interval<10;15>", new CellFactory(_WindsorContainer).CreateCell("ReturnValue1", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", "INTERVAL<15;24)", new CellFactory(_WindsorContainer).CreateCell("ReturnValue2", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", "INTERVAL(50;100)", new CellFactory(_WindsorContainer).CreateCell("ReturnValue3", CellInputOutputType.Output));
            Assert.AreEqual(2, items.FindAll("Anything", "Anything2", "Anything3", "MyString", 15m).Count);
        }

        [Test]
        public void IntegrityTest_Find_OneFromMoreRuleItems_IntervalStringAsCellValidateFilterDecimal_ReturnsCorrectOutputValue_CalculateResultValueByNCalc()
        {
            RuleItems items = new RuleItems(_WindsorContainer);
            items.AddRuleItem(".*", ".*", ".*", "MyString", "Interval<10;15)", new CellFactory(_WindsorContainer).CreateCell("C2/400", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", "INTERVAL<15;24)", new CellFactory(_WindsorContainer).CreateCell("C2/240", CellInputOutputType.Output));
            var filterValue = items.Find("Anything", "Anything2", "Anything3", "MyString", 15m).Output(0).FilterValue;

            var expr = new Expression(filterValue.ToString());
            expr.Parameters["C2"] = 480;
            var actual = expr.Evaluate();
            Assert.AreEqual(2, actual);
        }

        //[Test]
        //public void MemoryTest()
        //{
        //    for (int i = 0; i < 100000; i++)
        //    {
        //        RuleItems items = new RuleItems(_WindsorContainer);
        //        items.AddRuleItem(".*", ".*", ".*", "MyString", "Interval<10;15)", new CellFactory(_WindsorContainer).CreateCell("C2/400", CellInputOutputType.Output));
        //        items.AddRuleItem(".*", ".*", ".*", "MyString", "INTERVAL<15;24)", new CellFactory(_WindsorContainer).CreateCell("C2/240", CellInputOutputType.Output));
        //        Debug.WriteLine($"{i} : {GC.GetTotalMemory(true)}");
        //    }
        //}
    }
}
