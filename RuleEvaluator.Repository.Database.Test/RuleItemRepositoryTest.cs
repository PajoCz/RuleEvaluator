using System.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using NUnit.Framework;

namespace RuleEvaluator.Repository.Database.Test
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

        [Test]
        public void IntegrityTest_RepoLoad_FindOneRuleItemAndCheckFilterValue()
        {
            //Arrange
            var cf = _WindsorContainer.Resolve<ICellFactory>();

            //Act
            var repo = new RuleItemsRepository(cf, ConfigurationManager.AppSettings.Get("ConnectionString"), "Ciselnik.p_GetSchemaColBySchemaKod", "Ciselnik.p_GetTranslatorDataBySchemaKod");
            var items = repo.Load("OdhadBodu");
            var found = items.Find("A", "B", "C", "7BN Perspektiva Důchod", 15);
            var outputValue = found.Output(0).FilterValue;

            //Assert
            Assert.AreEqual("C2/240", outputValue);
        }
    }
}
