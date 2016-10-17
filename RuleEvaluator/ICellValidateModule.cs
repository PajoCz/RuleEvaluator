namespace RuleEvaluator
{
    public interface ICellValidateModule
    {
        bool? Validate(object p_CellFilter, object p_ValueDataForValidating);
    }
}