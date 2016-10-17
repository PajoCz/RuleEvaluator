using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace RuleEvaluator
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ICell>().ImplementedBy<Cell>().LifestyleTransient());

            //Modules in Chain of responsibility design pattern. 
            //Last module is CellValidateModuleRegex without any dependency (implements only ctor without ICellValidateModule parameter)
            container.Register(Component.For<ICellValidateModule>()
                     .ImplementedBy<CellValidateModuleDecimalInterval>().LifestyleSingleton()
                     .DependsOn(Dependency.OnComponent<ICellValidateModule, CellValidateModuleRegex>()),
                     Component.For<ICellValidateModule>()
                      .ImplementedBy<CellValidateModuleRegex>().LifestyleSingleton());
        }
    }
}
