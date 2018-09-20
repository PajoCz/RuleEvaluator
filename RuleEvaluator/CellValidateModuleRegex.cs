using System;
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

        public bool? Validate(object p_CellFilter, object p_ValueDataForValidating)
        {
            if (p_CellFilter == null) throw new ArgumentNullException(nameof(p_CellFilter));

            //always try Regex. Must be last Chainable module.

            return new Regex("^" + p_CellFilter + "$", RegexOptions.Singleline).IsMatch(p_ValueDataForValidating?.ToString() ?? String.Empty);
        }
    }
}