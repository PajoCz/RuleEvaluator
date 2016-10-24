# RuleEvaluator

## Quick links

|Item                  |Link                                                                                  |
|:---------------------|:-------------------------------------------------------------------------------------|
|Code coverage         | [![Coverage Status](https://coveralls.io/repos/github/PajoCz/RuleEvaluator/badge.svg?branch=master)](https://coveralls.io/github/PajoCz/RuleEvaluator?branch=master) |
|NuGet                 |  [![NuGet version (RuleEvaluator)](https://img.shields.io/nuget/v/RuleEvaluator.svg?style=flat-square)](https://www.nuget.org/packages/RuleEvaluator/)
|NuGet Repository      |  [![NuGet version (RuleEvaluator.Repository.Database)](https://img.shields.io/nuget/v/RuleEvaluator.Repository.Database.svg?style=flat-square)](https://www.nuget.org/packages/RuleEvaluator.Repository.Database/)


For usage, see UnitTests. One sample is:

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
        public void IntegrityTest_Find_OneFromMoreRuleItems_IntervalStringAsCellValidateFilterDecimal_ReturnsCorrectOutputValue()
        {
            RuleItems items = new RuleItems(_WindsorContainer);
            items.AddRuleItem(".*", ".*", ".*", "MyString", "Interval<10;15)", new CellFactory(_WindsorContainer).CreateCell("ReturnValue1", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", ".*", "MyString", "INTERVAL<15;24)", new CellFactory(_WindsorContainer).CreateCell("ReturnValue2", CellInputOutputType.Output));
            Assert.AreEqual("ReturnValue2", items.Find("Anything", "Anything2", "Anything3", "MyString", 15m).Output(0).FilterValue);
        }
