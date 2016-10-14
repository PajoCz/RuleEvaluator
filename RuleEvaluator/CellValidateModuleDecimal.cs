using System;

namespace RuleEvaluator
{
    public class CellValidateModuleDecimal : ICellValidateModule
    {
        public bool Validate(object p_CellFilter, object p_ValueDataForValidating)
        {
            if (p_CellFilter == null) throw new ArgumentNullException(nameof(p_CellFilter));
            if (p_ValueDataForValidating == null) throw new ArgumentNullException(nameof(p_ValueDataForValidating));
            if (!(p_CellFilter is CellValidateFilterDecimal)) throw new ArgumentException($"{nameof(p_CellFilter)} is not of expected type {typeof(CellValidateFilterDecimal)}");
            if (!(p_ValueDataForValidating is decimal)) throw new ArgumentException($"{nameof(p_ValueDataForValidating)} is not of expected type {typeof(decimal)}");

            var filter = (CellValidateFilterDecimal) p_CellFilter;
            decimal num = (decimal) p_ValueDataForValidating;
            return filter.Validate(num);
        }
    }
}