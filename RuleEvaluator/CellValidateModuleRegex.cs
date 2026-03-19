using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace RuleEvaluator
{
    /// <summary>
    /// Common validate module - implemented by Regular expression
    /// </summary>
    public class CellValidateModuleRegex: ICellValidateModule
    {
        private readonly ConcurrentDictionary<string, Regex> _regexCache = new ConcurrentDictionary<string, Regex>();

        public bool? Validate(object p_CellFilter, object p_ValueDataForValidating)
        {
            if (p_CellFilter == null) throw new ArgumentNullException(nameof(p_CellFilter));

            var regex = _regexCache.GetOrAdd(p_CellFilter.ToString()!, cellFilter => new Regex("^" + cellFilter + "$", RegexOptions.Singleline));
            return regex.IsMatch(p_ValueDataForValidating?.ToString() ?? string.Empty);
        }
    }
}