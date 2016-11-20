namespace RuleEvaluator
{    
    public interface ICellFactory
    {
        ICell CreateCell(object p_FilterValue, CellInputOutputType p_CellInputOutputTypeType = CellInputOutputType.Input);
    }
}