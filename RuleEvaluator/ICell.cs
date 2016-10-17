namespace RuleEvaluator
{
    public interface ICell
    {
        object FilterValue { get; set; }
        CellInputOutputType InputOutputType { get; set; }
        bool Validate(object p_Value);
    }
}