using System;

namespace RuleEvaluator
{
    public class Cell : ICell
    {
        /// <summary>
        /// IoC injected validate module
        /// </summary>
        private readonly ICellValidateModule _CellValidateModule;
        /// <summary>
        /// Cell filter value
        /// </summary>
        public object FilterValue { get; set; }
        /// <summary>
        /// Input cell value means used for looking in Validate method. Output cell means used for getting data from Validated RuleItem
        /// </summary>
        public CellInputOutputType InputOutputType { get; set; }

        public Cell(ICellValidateModule p_CellValidateModule)
        {
            _CellValidateModule = p_CellValidateModule;
        }

        public bool Validate(object p_Value)
        {
            if (FilterValue == null) throw new ArgumentNullException(nameof(FilterValue));

            bool? res = _CellValidateModule.Validate(FilterValue, p_Value);
            if (!res.HasValue)
            {
                throw new Exception("Uknown validate result from ICellValidateModule instances");
            }
            return res.Value;
        }
    }
}