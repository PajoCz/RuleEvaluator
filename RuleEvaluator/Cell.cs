using System;

namespace RuleEvaluator
{
    public class Cell : ICell
    {
        private readonly ICellValidateModule _cellValidateModule;

        /// <summary>
        /// Cell filter value (the pattern or value to match against).
        /// </summary>
        public object FilterValue { get; set; }

        /// <summary>
        /// Whether this cell is Input, Output, or PrimaryKey.
        /// </summary>
        public CellInputOutputType InputOutputType { get; set; }

        public Cell(ICellValidateModule p_CellValidateModule, object p_FilterValue, CellInputOutputType p_CellInputOutputTypeType = CellInputOutputType.Input)
        {
            _cellValidateModule = p_CellValidateModule ?? throw new ArgumentNullException(nameof(p_CellValidateModule));
            FilterValue = p_FilterValue;
            InputOutputType = p_CellInputOutputTypeType;
        }

        public bool Validate(object p_Value)
        {
            if (FilterValue == null) throw new ArgumentNullException(nameof(FilterValue));

            bool? res = _cellValidateModule.Validate(FilterValue, p_Value);
            if (!res.HasValue)
            {
                throw new MatcherException("Unknown validate result from ICellValidateModule instances");
            }
            return res.Value;
        }
    }
}