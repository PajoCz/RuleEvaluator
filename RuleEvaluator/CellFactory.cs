using Castle.Windsor;

namespace RuleEvaluator
{
    /// <summary>
    /// Factory for creating ICell objects. Implemented by IWindsorContainer
    /// </summary>
    public class CellFactory
    {
        private readonly IWindsorContainer _Container;

        public CellFactory(IWindsorContainer p_Container)
        {
            _Container = p_Container;
        }

        public ICell CreateCell(object p_FilterValue, CellInputOutputType p_CellInputOutputTypeType = CellInputOutputType.Input)
        {
            ICell cell = _Container.Resolve<ICell>();
            cell.FilterValue = p_FilterValue;
            cell.InputOutputType = p_CellInputOutputTypeType;
            return cell;
        }
    }
}
