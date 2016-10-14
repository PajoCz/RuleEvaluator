namespace RuleEvaluator
{
    [CellValidateFilter(ValidateModule = typeof(CellValidateModuleDecimal))]
    public class CellValidateFilterDecimal
    {
        public CellValidateFilterDecimal(decimal p_From, bool p_FromIncluding, decimal p_To, bool p_ToIncluding)
        {
            From = p_From;
            FromIncluding = p_FromIncluding;
            To = p_To;
            ToIncluding = p_ToIncluding;
        }

        public CellValidateFilterDecimal()
        {
        }

        public decimal From { get; set; }
        public bool FromIncluding { get; set; }
        public decimal To { get; set; }
        public bool ToIncluding { get; set; }

        public bool Validate(decimal p_Value)
        {
            var result = true;
            if (FromIncluding && p_Value < From) result = false;
            if (!FromIncluding && p_Value <= From) result = false;
            if (ToIncluding && p_Value > To) result = false;
            if (!ToIncluding && p_Value >= To) result = false;
            return result;
        }
    }
}