using System;

namespace RuleEvaluator
{
    public class Cell
    {
        public readonly ICellValidate CellValidateModule;
        public readonly object FilterValue;
        public readonly CellInputOutputType InputOutputType;

        public Cell(object p_FilterValue, CellInputOutputType p_CellInputOutputType = CellInputOutputType.Input)
            : this(p_FilterValue, new CellValidateRegex(), p_CellInputOutputType)
        {
        }

        public Cell(object p_FilterValue, ICellValidate p_CellValidateModule, CellInputOutputType p_CellInputOutputTypeType = CellInputOutputType.Input)
        {
            if (p_FilterValue == null) throw new ArgumentNullException(nameof(p_FilterValue));

            FilterValue = p_FilterValue;
            CellValidateModule = p_CellValidateModule;
            InputOutputType = p_CellInputOutputTypeType;
        }

        public bool Validate(object p_Value)
        {
            return CellValidateModule.Validate(FilterValue, p_Value);
            //return FilterValue.Equals(p_FilterValue);
        }
    }
}