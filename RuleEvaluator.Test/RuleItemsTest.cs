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
            //Arrange
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            RuleItems items = new RuleItems(cf);
            items.AddRuleItem(".*", ".*", ".*", "MyString", "1[0-4]", cf.CreateCell("ReturnValue1", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", "1[5-9]|2[0-4]", cf.CreateCell("ReturnValue2", CellInputOutputType.Output));

            //Act
            var found = items.Find("Anything", "Anything2", "Anything3", "MyString", "15").Output(0).FilterValue;

            //Assert
            Assert.AreEqual("ReturnValue2", found);
        }

        [Test]
        public void IntegrityTest_Find_OneFromMoreRuleItems_CellValidateDecimalByDetector_ReturnsCorrectOutputValue()
        {
            //Arrange
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            RuleItems items = new RuleItems(cf);
            items.AddRuleItem(".*", ".*", ".*", "MyString", cf.CreateCell(new CellValidateFilterDecimalInterval(10, true, 15, false)),
                cf.CreateCell("ReturnValue1", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", cf.CreateCell(new CellValidateFilterDecimalInterval(15, true, 24, false)),
                cf.CreateCell("ReturnValue2", CellInputOutputType.Output));

            //Act
            var found = items.Find("Anything", "Anything2", "Anything3", "MyString", 20m).Output(0).FilterValue;

            //Assert
            Assert.AreEqual("ReturnValue2", found);
        }

        [Test]
        public void IntegrityTest_Find_OneFromMoreRuleItems_IntervalStringAsCellValidateFilterDecimal_ReturnsCorrectOutputValue()
        {
            //Arrange
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            RuleItems items = new RuleItems(cf);
            items.AddRuleItem(".*", ".*", ".*", "MyString", "Interval<10;15)", cf.CreateCell("ReturnValue1", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", "INTERVAL<15;24)", cf.CreateCell("ReturnValue2", CellInputOutputType.Output));

            //Act
            var found = items.Find("Anything", "Anything2", "Anything3", "MyString", 15m).Output(0).FilterValue;

            //Assert
            Assert.AreEqual("ReturnValue2", found);
        }

        [Test]
        public void IntegrityTest_FindAll()
        {
            //Arrange
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            RuleItems items = new RuleItems(cf);
            items.AddRuleItem(".*", ".*", ".*", "MyString", "Interval<10;15>", cf.CreateCell("ReturnValue1", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", "INTERVAL<15;24)", cf.CreateCell("ReturnValue2", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", "INTERVAL(50;100)", cf.CreateCell("ReturnValue3", CellInputOutputType.Output));

            //Act
            var count = items.FindAll("Anything", "Anything2", "Anything3", "MyString", 15m).Count;

            //Assert
            Assert.AreEqual(2, count);
        }

        [Test]
        public void IntegrityTest_Find_OneFromMoreRuleItems_IntervalStringAsCellValidateFilterDecimal_ReturnsCorrectOutputValue_CalculateResultValueByNCalc()
        {
            //Arrange
            var cf = _WindsorContainer.Resolve<ICellFactory>();
            RuleItems items = new RuleItems(cf);
            items.AddRuleItem(".*", ".*", ".*", "MyString", "Interval<10;15)", cf.CreateCell("C2/400", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", "INTERVAL<15;24)", cf.CreateCell("C2/240", CellInputOutputType.Output));

            //Act - Find
            var filterValue = items.Find("Anything", "Anything2", "Anything3", "MyString", 15m).Output(0).FilterValue;

            //Act - calculate expression by NCalc
            var expr = new Expression(filterValue.ToString())
            {
                Parameters =
                {
                    ["C1"] = null,
                    ["C2"] = 36000,
                    ["C3"] = null,
                    ["C4"] = null,
                    ["C5"] = null
                }
            };
            var actual = expr.Evaluate();

            //Assert
            Assert.AreEqual(150, actual);
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
