namespace RuleEvaluator
{
    public interface ICellValidate
    {
        bool Validate(object p_CellFilter, object p_ValueDataForValidating);
    }
}