using System;
using System.Text.RegularExpressions;

namespace RuleEvaluator
{
    public class CellValidateModuleRegex: ICellValidateModule
    {
        public bool Validate(object p_CellFilter, object p_ValueDataForValidating)
        {
            if (p_CellFilter == null) throw new ArgumentNullException(nameof(p_CellFilter));

            return new Regex("^" + p_CellFilter + "$").IsMatch(p_ValueDataForValidating?.ToString() ?? String.Empty);
        }
    }
}