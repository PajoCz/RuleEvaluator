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

RuleEvaluator.Repository.Database can load RuleItems from DB. One unit test sample looks like:

        [Test]
        public void IntegrateTest_RepoLoad_FindOneRuleItemAndCheckFilterValue()
        {
            var repo = new RuleItemsRepository(_WindsorContainer, ConfigurationManager.AppSettings.Get("ConnectionString"), "Ciselnik.p_GetSchemaColBySchemaKod", "Ciselnik.p_GetTranslatorDataBySchemaKod");
            var items = repo.Load("OdhadBodu");
            var found = items.Find("A", "B", "C", "7BN Perspektiva Důchod", 15);
            var outputValue = found.Output(0).FilterValue;
            Assert.AreEqual("C2/240", outputValue);
        }
