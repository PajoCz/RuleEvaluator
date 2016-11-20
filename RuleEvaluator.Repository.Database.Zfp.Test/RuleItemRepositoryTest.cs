using NUnit.Framework;
using System.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace RuleEvaluator.Repository.Database.Zfp.Test
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
                return container;
            }
        }

        private static RuleItemsRepository RuleItemsRepository
        {
            get
            {
                var cf = _WindsorContainer.Resolve<ICellFactory>();
                var repo = new RuleItemsRepository(cf, ConfigurationManager.AppSettings.Get("ConnectionString"), "Ciselnik.p_GetSchemaColBySchemaKod",
                    "Ciselnik.p_GetTranslatorDataBySchemaKod");
                return repo;
            }
        }

        [Test]
        public void IntegrateTest_RepoLoad_FindOneRuleItemAndCheckFilterValue1()
        {
            var items = RuleItemsRepository.Load("OdhadBodu");
            var found = items.Find("ZFP akademie, a.s.", "", "", "ZFP Život + Unisex 2016", 27);
            var outputValue = found.Output(0).FilterValue;
            Assert.AreEqual("C2/200", outputValue);
        }

        [Test]
        public void IntegrateTest_RepoLoad_FindOneRuleItemAndCheckFilterValueWithMoreOut()
        {
            var items = RuleItemsRepository.Load("OdhadBoduViditelnostPolozek");
            var found = items.Find("ZFP akademie, a.s.", "", "", "ZFP Život + Unisex 2016");
            string c1 = found.Output(0).FilterValue.ToString();
            string c2 = found.Output(1).FilterValue.ToString();
            string c3 = found.Output(2).FilterValue.ToString();
            string c4 = found.Output(3).FilterValue.ToString();
            string c5 = found.Output(4).FilterValue.ToString();
            bool result = c1 == "" && c2 == "Roční pojistné" && c3 == "" && c4 == "" && c5 == "Pojistná doba";
            Assert.True(result);
        }

        [Test]
        public void IntegrateTest_RepoLoad_FindOneRuleItemAndCheckFilterValueWithMoreOut1()
        {
            var items = RuleItemsRepository.Load("OdhadBoduViditelnostPolozek");
            var found = items.Find("ZFP akademie, a.s.", "", "", "Cena koksu");
            string c1 = found.Output(0).FilterValue.ToString();
            string c2 = found.Output(1).FilterValue.ToString();
            string c3 = found.Output(2).FilterValue.ToString();
            string c4 = found.Output(3).FilterValue.ToString();
            string c5 = found.Output(4).FilterValue.ToString();
            bool result = c1 == "" && c2 == "" && c3 == "" && c4 == "" && c5 == "";
            Assert.True(result);
        }
    }
}
