using System;
using System.Reflection;

namespace RuleEvaluator
{
    public class Cell
    {
        public readonly ICellValidateModule CellValidateModuleModule;
        public readonly object FilterValue;
        public readonly CellInputOutputType InputOutputType;

        public Cell(object p_FilterValue, CellInputOutputType p_CellInputOutputType = CellInputOutputType.Input)
            : this(p_FilterValue, new CellValidateModuleRegex(), p_CellInputOutputType)
        {
        }

        public Cell(object p_FilterValue, ICellValidateModule p_CellValidateModuleModule, CellInputOutputType p_CellInputOutputTypeType = CellInputOutputType.Input)
        {
            if (p_FilterValue == null) throw new ArgumentNullException(nameof(p_FilterValue));

            FilterValue = p_FilterValue;
            CellValidateModuleModule = p_CellValidateModuleModule;
            InputOutputType = p_CellInputOutputTypeType;
        }

        public bool Validate(object p_Value)
        {
            return CellValidateModuleModule.Validate(FilterValue, p_Value);
            //return FilterValue.Equals(p_FilterValue);
        }
    }
}