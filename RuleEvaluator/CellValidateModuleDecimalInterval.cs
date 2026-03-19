using System;

namespace RuleEvaluator
{
    /// <summary>
    /// Module for Validate cell input by cell filter written in Interval format (correct string pattern or CellValidateFilterDecimalInterval class)
    /// </summary>
    public class CellValidateModuleDecimalInterval : ICellValidateModule
    {
        private readonly ICellValidateModule? _nextModule;

        public CellValidateModuleDecimalInterval()
        {
        }

        /// <summary>
        /// Constructor with next module in chain of responsibility.
        /// </summary>
        public CellValidateModuleDecimalInterval(ICellValidateModule p_NextModule)
        {
            _nextModule = p_NextModule;
        }

        public bool? Validate(object p_CellFilter, object p_ValueDataForValidating)
        {
            var cellFilter = p_CellFilter;
            if (cellFilter != null && !(cellFilter is CellValidateFilterDecimalInterval))
            {
                cellFilter = CellValidateFilterDecimalInterval.CreateFromString(p_CellFilter.ToString()!);
            }

            if (cellFilter == null && _nextModule != null)
            {   //try another module, p_CellFilter is in unknown format
                return _nextModule.Validate(p_CellFilter, p_ValueDataForValidating);
            }

            if (cellFilter == null) throw new ArgumentNullException(nameof(p_CellFilter));
            if (p_ValueDataForValidating == null) throw new ArgumentNullException(nameof(p_ValueDataForValidating));
            if (!(p_ValueDataForValidating is decimal))
            {
                p_ValueDataForValidating = Convert.ToDecimal(p_ValueDataForValidating);
            }

            var filter = (CellValidateFilterDecimalInterval)cellFilter;
            decimal num = (decimal) p_ValueDataForValidating;
            return filter.Validate(num);
        }
    }
}