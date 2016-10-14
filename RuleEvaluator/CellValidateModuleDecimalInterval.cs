using System;

namespace RuleEvaluator
{
    public class CellValidateModuleDecimalInterval : ICellValidateModule
    {
        public bool Validate(object p_CellFilter, object p_ValueDataForValidating)
        {
            if (p_CellFilter != null && !(p_CellFilter is CellValidateFilterDecimalInterval))
            {
                p_CellFilter = CellValidateFilterDecimalInterval.CreateFromString(p_CellFilter.ToString());
            }

            if (p_CellFilter == null) throw new ArgumentNullException(nameof(p_CellFilter));
            if (p_ValueDataForValidating == null) throw new ArgumentNullException(nameof(p_ValueDataForValidating));
            if (!(p_ValueDataForValidating is decimal)) throw new ArgumentException($"{nameof(p_ValueDataForValidating)} is not of expected type {typeof(decimal)}");

            var filter = (CellValidateFilterDecimalInterval) p_CellFilter;
            decimal num = (decimal) p_ValueDataForValidating;
            return filter.Validate(num);
        }
    }
}