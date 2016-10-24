using System;

namespace RuleEvaluator
{
    /// <summary>
    /// Module for Validate cell input by cell filter written in Interval format (correct string pattern or CellValidateFilterDecimalInterval class)
    /// </summary>
    public class CellValidateModuleDecimalInterval : ICellValidateModule
    {
        private readonly ICellValidateModule _NextModule;

        public CellValidateModuleDecimalInterval()
        {
        }

        /// <summary>
        /// ctor with ICellValidateModule is used by Windsor Castle. This module is used in Chain of Responsibility design pattern.
        /// </summary>
        /// <param name="p_NextModule"></param>
        public CellValidateModuleDecimalInterval(ICellValidateModule p_NextModule)
        {
            _NextModule = p_NextModule;
        }

        public bool? Validate(object p_CellFilter, object p_ValueDataForValidating)
        {
            var cellFilter = p_CellFilter;
            if (cellFilter != null && !(cellFilter is CellValidateFilterDecimalInterval))
            {
                cellFilter = CellValidateFilterDecimalInterval.CreateFromString(p_CellFilter.ToString());
            }

            if (cellFilter == null && _NextModule != null)
            {   //try another module, p_CellFilter is in unknown format
                return _NextModule.Validate(p_CellFilter, p_ValueDataForValidating);
            }

            if (cellFilter == null) throw new ArgumentNullException(nameof(p_CellFilter));
            if (p_ValueDataForValidating == null) throw new ArgumentNullException(nameof(p_ValueDataForValidating));
            if (p_ValueDataForValidating is int)
            {
                p_ValueDataForValidating = Convert.ToDecimal(p_ValueDataForValidating);
            }
            if (!(p_ValueDataForValidating is decimal)) throw new ArgumentException($"{nameof(p_ValueDataForValidating)} is not of expected type {typeof(decimal)}");

            var filter = (CellValidateFilterDecimalInterval)cellFilter;
            decimal num = (decimal) p_ValueDataForValidating;
            return filter.Validate(num);
        }
    }
}