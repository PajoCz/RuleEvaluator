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
        //always try Regex. Must be last Chainable module.
        //private readonly ICellValidateModule _NextModule;

        //public CellValidateModuleRegex(ICellValidateModule p_NextModule)
        //{
        //    _NextModule = p_NextModule;
        //}

        private ConcurrentDictionary<string, Regex> _RegexCache = new ConcurrentDictionary<string, Regex>();
        
        public bool? Validate(object p_CellFilter, object p_ValueDataForValidating)
        {
            if (p_CellFilter == null) throw new ArgumentNullException(nameof(p_CellFilter));

            //always try Regex. Must be last Chainable module.

            var regex = _RegexCache.GetOrAdd(p_CellFilter.ToString(), cellFilter => new Regex("^" + cellFilter + "$", RegexOptions.Singleline));
            return regex.IsMatch(p_ValueDataForValidating?.ToString() ?? string.Empty);
        }
    }
}